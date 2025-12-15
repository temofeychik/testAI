using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuotesService.Models;

public class Quote
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
