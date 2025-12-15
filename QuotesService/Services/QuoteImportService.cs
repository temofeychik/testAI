using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuotesService.Data;
using QuotesService.Models;

namespace QuotesService.Services;

public class QuoteImportService
{
    private readonly HttpClient _httpClient;
    private readonly QuotesContext _context;
    private readonly ILogger<QuoteImportService> _logger;
    private readonly string _initialUrl;

    public QuoteImportService(HttpClient httpClient, QuotesContext context, ILogger<QuoteImportService> logger, IOptions<PaperQuotesOptions> options)
    {
        _httpClient = httpClient;
        _context = context;
        _logger = logger;
        _initialUrl = options.Value.BaseUrl;
    }

    public async Task<int> ImportQuotesAsync(CancellationToken cancellationToken = default)
    {
        var existingIds = await _context.Quotes.Select(q => q.Id).ToHashSetAsync(cancellationToken);
        var added = 0;
        var nextUrl = _initialUrl;

        while (!string.IsNullOrWhiteSpace(nextUrl))
        {
            var response = await _httpClient.GetFromJsonAsync<PaperQuotesResponse>(nextUrl, cancellationToken);
            if (response?.Results is null || response.Results.Count == 0)
            {
                _logger.LogInformation("No quotes found at {Url}", nextUrl);
                break;
            }

            foreach (var item in response.Results)
            {
                if (existingIds.Contains(item.Pk))
                {
                    continue;
                }

                _context.Quotes.Add(new Quote
                {
                    Id = item.Pk,
                    Text = item.Quote,
                    Author = item.Author,
                    Likes = item.Likes,
                    Tags = string.Join(',', item.Tags ?? Array.Empty<string>()),
                    Image = item.Image,
                    Language = item.Language
                });

                existingIds.Add(item.Pk);
                added++;
            }

            await _context.SaveChangesAsync(cancellationToken);
            nextUrl = response.Next;
        }

        return added;
    }
}

public record PaperQuotesOptions
{
    public string BaseUrl { get; init; } = string.Empty;
}

public record PaperQuotesResponse
{
    public string? Next { get; init; }
    public string? Previous { get; init; }
    public List<PaperQuoteDto> Results { get; init; } = new();
}

public record PaperQuoteDto
{
    public string Quote { get; init; } = string.Empty;
    public string? Author { get; init; }
    public int Likes { get; init; }
    public List<string>? Tags { get; init; }
    public int Pk { get; init; }
    public string? Image { get; init; }
    public string Language { get; init; } = string.Empty;
}
