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

        // Data
        services.AddScoped<OrdersContext>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();

        // Application
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IVoucherQueries, VoucherQueries>();
        services.AddScoped<IOrderQueries, OrderQueries>();

        services.AddMediatR(typeof(DependencyInjectionConfig));
    }
}
