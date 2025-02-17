using ElasticSearch.WEB.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureApplicationServices(builder.Configuration);

var app = builder.Build();
app.ConfigureRequestPipeline();
await app.RunAsync();