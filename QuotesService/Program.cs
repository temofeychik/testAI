using Microsoft.EntityFrameworkCore;
using QuotesService.Data;
using QuotesService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<PaperQuotesOptions>(builder.Configuration.GetSection("PaperQuotes"));

builder.Services.AddDbContext<QuotesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<QuoteImportService>();
builder.Services.AddScoped<QuoteImportService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
