using Microsoft.AspNetCore.Authorization;

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

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter the token JWT like this: Bearer {your token}",
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
     });
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<CatalogContext>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddAuthorizationBuilder()
  .AddPolicy("catalog", policy =>
        policy
            .RequireClaim("Catalog", "Read"));
            

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthConfiguration();

app.MapControllers();

app.MapGet("catalog/products", (IProductRepository productRepository) =>
    productRepository.GetAllAsync())
    .AllowAnonymous();

app.MapGet("catalog/products/{id:guid}", (IProductRepository productRepository, Guid id) =>
    productRepository.GetAsync(id))
    .RequireAuthorization("catalog");

app.Run();
