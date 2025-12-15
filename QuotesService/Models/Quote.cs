using System.ComponentModel.DataAnnotations;

namespace QuotesService.Models;

public class Quote
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Text { get; set; } = string.Empty;

    public string? Author { get; set; }

    public int Likes { get; set; }

    public string Tags { get; set; } = string.Empty;

    public string? Image { get; set; }

    [Required]
    public string Language { get; set; } = string.Empty;
}
