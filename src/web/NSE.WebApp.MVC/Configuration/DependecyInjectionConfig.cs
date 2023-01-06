namespace NSE.WebApp.MVC.Configuration;

public static class DependecyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddHttpClient<IAuthenticationService, AuthenticationService>();
    }
}