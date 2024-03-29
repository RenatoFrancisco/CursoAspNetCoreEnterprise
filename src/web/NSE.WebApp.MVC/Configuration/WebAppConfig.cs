namespace NSE.WebApp.MVC.Configuration;

public static class WebAppConfig
{    
    public static void AddMvcConfiguration(this IServiceCollection services, 
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

        services.AddControllersWithViews();

        services.Configure<AppSettings>(configuration);
    }

    public static void UseMvcConfiguration(this WebApplication app)
    {
        //if (!app.Environment.IsDevelopment())
        //{
        //    app.UseExceptionHandler("/error/500");
        //    app.UseStatusCodePagesWithRedirects("/error/{0}");
        //    app.UseHsts();
        //}

        app.UseExceptionHandler("/error/500");
        app.UseStatusCodePagesWithRedirects("/error/{0}");
        app.UseHsts();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityConfiguration();

        var supportedCultures = new[] { new CultureInfo("pt-BR") };
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("pt-BR"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        app.UseMiddleware<ExceptionMiddleware>();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Catalog}/{action=Index}/{id?}");

        app.Run();
    }
}