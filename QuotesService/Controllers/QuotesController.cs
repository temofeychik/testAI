using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuotesService.Data;
using QuotesService.Models;
using QuotesService.Services;
using System.ComponentModel.DataAnnotations;

namespace QuotesService.Controllers;

[ApiController]
[Route("api/[controller]")]
    public class QuotesController : ControllerBase
    {
        private readonly QuotesContext _context;
        private readonly QuoteImportService _importService;

    public QuotesController(QuotesContext context, QuoteImportService importService)
    {
        _context = context;
        _importService = importService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Quote>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var quotes = await _context.Quotes.AsNoTracking().OrderBy(q => q.Id).ToListAsync(cancellationToken);
        return Ok(quotes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Quote>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var quote = await _context.Quotes.AsNoTracking().FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
        if (quote is null)
        {
            return NotFound();
        }

        return Ok(quote);
    }

    [HttpPost("import")]
    public async Task<ActionResult<string>> ImportAsync(CancellationToken cancellationToken)
    {
        var added = await _importService.ImportQuotesAsync(cancellationToken);
        return Ok($"Imported {added} quotes.");
    }

    [HttpPost]
    public async Task<ActionResult<Quote>> CreateAsync([FromBody] QuotePayload payload, CancellationToken cancellationToken)
    {
        var id = payload.Id ?? await NextIdAsync(cancellationToken);

        if (await _context.Quotes.AnyAsync(q => q.Id == id, cancellationToken))
        {
            return Conflict($"Quote with id {id} already exists.");
        }

        var quote = MapToEntity(payload, id);
        _context.Quotes.Add(quote);
        await _context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = quote.Id }, quote);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Quote>> UpdateAsync(int id, [FromBody] QuotePayload payload, CancellationToken cancellationToken)
    {
        var quote = await _context.Quotes.FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
        if (quote is null)
        {
            return NotFound();
        }

        quote.Text = payload.Text;
        quote.Author = payload.Author;
        quote.Likes = payload.Likes;
        quote.Tags = payload.Tags ?? string.Empty;
        quote.Image = payload.Image;
        quote.Language = payload.Language;

        await _context.SaveChangesAsync(cancellationToken);

        return Ok(quote);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var quote = await _context.Quotes.FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
        if (quote is null)
        {
            return NotFound();
        }

        _context.Quotes.Remove(quote);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private static Quote MapToEntity(QuotePayload payload, int id) => new()
    {
        Id = id,
        Text = payload.Text,
        Author = payload.Author,
        Likes = payload.Likes,
        Tags = payload.Tags ?? string.Empty,
        Image = payload.Image,
        Language = payload.Language
    };

    private async Task<int> NextIdAsync(CancellationToken cancellationToken)
    {
        var maxId = await _context.Quotes.Select(q => (int?)q.Id).MaxAsync(cancellationToken);
        return (maxId ?? 0) + 1;
    }
}

public record QuotePayload
{
    public int? Id { get; init; }

    [Required]
    public string Text { get; init; } = string.Empty;

    public string? Author { get; init; }

    [Range(0, int.MaxValue)]
    public int Likes { get; init; }

    public string? Tags { get; init; }

    public string? Image { get; init; }

    [Required]
    public string Language { get; init; } = "ru";
}
