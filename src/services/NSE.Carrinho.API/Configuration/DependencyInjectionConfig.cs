namespace NSE.Carrinho.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<AspNetUser, AspNetUser>();
        services.AddScoped<CartContext>();
    }
}
