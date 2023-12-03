using CatalogService.Api.Extensions;
using CatalogService.Api.Infrastructure.Context;
using CatalogService.Api.Infrastructure.Setup;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    WebRootPath = "Pics"
});

builder.Services.Configure<CatalogSettings>(builder.Configuration.GetSection("CatalogSettings"));
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Host.UseContentRoot(Directory.GetCurrentDirectory());

// Add services to the container.
builder.Services.ConfigureConsul(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MigrateDbContext<CatalogContext>((context, services) =>
{
    var env = services.GetService<IWebHostEnvironment>();
    var logger = services.GetService<ILogger<CatalogContextSeed>>();

    new CatalogContextSeed()
    .SeedAsync(context, env, logger)
    .Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.RegisterWithConsul(app.Lifetime);

app.Run();
