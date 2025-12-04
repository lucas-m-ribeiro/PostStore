using Microsoft.EntityFrameworkCore;
using Poststore.Data;

var builder = WebApplication.CreateBuilder(args);

//obtendo a connection string do appSettngs.json
//se estiver nulo, lança a exception
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//configurando o contexto para usar o PostgreSQL
builder.Services.AddDbContext<AppDbContext>(x =>
    x.UseNpgsql(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
