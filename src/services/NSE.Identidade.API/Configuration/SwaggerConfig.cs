namespace NSE.Identidade.API.Configuration;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options => 
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = $"NerdStore Enteprise Identity API - {builder.Environment.EnvironmentName}",
                Description = "This API makes parts of the Course ASP.NET Core Enterprise Applications"
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfig(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "v1"));
        }

        return app;
    }
}