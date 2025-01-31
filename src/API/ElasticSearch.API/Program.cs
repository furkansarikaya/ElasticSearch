using System.Reflection;
using ElasticSearch.API.Extensions;
using ElasticSearch.API.Repositories;
using ElasticSearch.API.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi(); // Your custom method
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ProductService>();

// Configure ElasticSearch
builder.Services.AddElastic(builder.Configuration); // Pass IConfiguration

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();