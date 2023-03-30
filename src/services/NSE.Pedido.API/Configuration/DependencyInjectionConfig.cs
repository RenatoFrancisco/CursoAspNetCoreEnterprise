namespace NSE.Pedido.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        // API
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        // Application
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        // Data
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<OrdersContext>();
    }
}
