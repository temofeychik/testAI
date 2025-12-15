using Microsoft.EntityFrameworkCore;
using QuotesService.Models;

namespace QuotesService.Data;

public class QuotesContext : DbContext
{
    public QuotesContext(DbContextOptions<QuotesContext> options) : base(options)
    {
    }

    public DbSet<Quote> Quotes => Set<Quote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quote>().Property(q => q.Tags).HasMaxLength(1024);
        base.OnModelCreating(modelBuilder);
    }
}
