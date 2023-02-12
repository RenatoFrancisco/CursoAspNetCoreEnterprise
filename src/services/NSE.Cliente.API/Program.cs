using NSE.Cliente.API.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<CustomersContext>(options =>
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


builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
builder.Services.AddScoped<IRequestHandler<RegisterCustomerCommand, ValidationResult>, CustomerCommandHandler>();

builder.Services.AddScoped<CustomersContext>();
builder.Services.AddScoped<ICustomerRepository, CustomerRespository>();

builder.Services.AddScoped<INotificationHandler<RegisteredCustomerEvent>, CustomerEventHandler>();

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = $"NerdStore Enteprise Customers API - {builder.Environment.EnvironmentName}",
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
            Array.Empty<string>()
        }
     });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
