using Microsoft.EntityFrameworkCore;
using Poststore.Data;
using Poststore.Models;

var builder = WebApplication.CreateBuilder(args);

//obtendo a connection string do appSettngs.json
//se estiver nulo, lança a exception
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//configurando o contexto para usar o PostgreSQL
builder.Services.AddDbContext<AppDbContext>(x =>
    x.UseNpgsql(connectionString));

var app = builder.Build();

//realiza um create de Categories
app.MapPost("/v1/categories", async  (AppDbContext context, Category category) => 
{
    {
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
        return Results.Ok(category);
    }
}
);

//realiza um get de produtos
app.MapGet("/v1/categories", async  (AppDbContext context) => 
    {
        var categories = await context.Categories
        .AsNoTracking()
        .ToListAsync();
        return Results.Ok(categories);
    }
);

app.Run();
