var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocaDbConnection")));

builder.Services.AddControllers();

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Total", builder =>
        builder
            .AllowAnyOrigin()
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = $"NerdStore Enteprise Catalog API - {builder.Environment.EnvironmentName}",
        Description = "This API makes parts of the Course ASP.NET Core Enterprise Applications"
    });
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<CatalogContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

app.UseAuthConfiguration();

app.MapControllers();

app.MapGet("catalog/products", (IProductRepository productRepository) =>
    productRepository.GetAllAsync());

app.MapGet("catalog/products/{id:guid}", (IProductRepository productRepository, Guid id) =>
    productRepository.GetAsync(id));

app.Run();
