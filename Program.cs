using Microsoft.EntityFrameworkCore;
using Poststore.Data;
using Poststore.Models;
using Poststore.Request;

var builder = WebApplication.CreateBuilder(args);

//obtendo a connection string do appSettngs.json
//se estiver nulo, lança a exception
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//configurando o contexto para usar o PostgreSQL
builder.Services.AddDbContext<AppDbContext>(x =>
    x.UseNpgsql(connectionString));

var app = builder.Build();

app.MapPost("/v1/categories", async (AppDbContext context, CreateCategoryRequest request) =>
{
    var category = new Category
    {
        Heading = new Heading
        {
            Title = request.Title,
            Slug = request.Slug
        }
    };
    await context.Categories.AddAsync(category);
    await context.SaveChangesAsync();
    return Results.Created();
});

app.MapGet("/v1/categories", async (AppDbContext context) =>
{
    var products = await context
        .Categories
        .AsNoTracking()
        .ToListAsync();
    return Results.Ok(products);
});

app.MapPost("/v1/products", async (AppDbContext context, ProductRequest request) =>
{
    var product = new Product
    {
        CategoryId = request.CategoryId,

        Heading = new Heading
        {
            Title = request.Title,
            Slug = request.Slug,
        }
    };
    await context.Products.AddAsync(product);
    await context.SaveChangesAsync();
    return Results.Created();
});

app.MapGet("/v1/products", async (AppDbContext context) =>
{
    var products = await context
        .Products
        .AsNoTracking()
        .ToListAsync();
    return Results.Ok(products);
});

app.Run();
