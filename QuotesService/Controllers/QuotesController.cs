using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuotesService.Data;
using QuotesService.Models;
using QuotesService.Services;

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

    [HttpPost("import")]
    public async Task<ActionResult<string>> ImportAsync(CancellationToken cancellationToken)
    {
        var added = await _importService.ImportQuotesAsync(cancellationToken);
        return Ok($"Imported {added} quotes.");
    }
}
