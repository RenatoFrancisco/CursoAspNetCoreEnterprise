namespace NSE.Pagamento.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        // API
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IPaymentFacade, CreditCardPaymentFacade>();

        services.AddScoped<IPaymentRepository, IPaymentRepository>();
        services.AddScoped<PaymentsContext>();
    }
}
