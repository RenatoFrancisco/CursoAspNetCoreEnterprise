using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Configuration;

public static class DependecyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddHttpClient<IAuthenticationService, AuthenticationService>();
        services.AddHttpClient<ICatalogService, CatalogService>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUser, AspNetUser>();
    }
}