namespace NSE.Bff.Compras.Configuration;

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

        services.Configure<AppServicesSettings>(configuration);

        services.AddControllers();
            //.ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

        services.AddCors(options =>
        {
            options.AddPolicy("Total",
                builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });

        return services;
    }

    public static IApplicationBuilder UseApiConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseHttpsRedirection();

        app.UseCors("Total");

        app.UseAuthConfiguration();

        app.MapControllers();

        app.Run();

        return app;
    }
}