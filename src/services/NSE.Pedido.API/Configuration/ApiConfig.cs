namespace NSE.Pedido.API.Configuration;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services,
                                                         ConfigurationManager configuration,
                                                         WebApplicationBuilder builder)
    {
        configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        if (builder.Environment.IsDevelopment())
            configuration.AddUserSecrets<Program>();

        services.AddControllers()
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

        services.AddDbContext<OrdersContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("LocaDbConnection")));

        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("Total",
                builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });

        services.AddAuthorizationBuilder()
          .AddPolicy("catalog", policy =>
                policy
                    .RequireClaim("Catalog", "Read"));

        return services;
    }

    public static void UseApiConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("Total");

        app.UseAuthConfiguration();

        app.MapControllers();

        app.Run();
    }
}
