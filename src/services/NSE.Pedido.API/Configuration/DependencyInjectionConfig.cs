namespace NSE.Pedido.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        // API
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        // Commands
        services.AddScoped<IRequestHandler<AddOrderCommand, ValidationResult>, OrderCommandHandler>();

        // Events
        services.AddScoped<INotificationHandler<ExecutedOrderEvent>, OrderEventHandler>();

        // Application
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IVoucherQueries, VoucherQueries>();
        services.AddScoped<IOrderQueries, OrderQueries>();

        // Data
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<OrdersContext>();
    }
}
