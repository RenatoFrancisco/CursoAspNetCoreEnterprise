namespace NSE.MessageBus;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException(nameof(connectionString));

        services.AddSingleton<IMessageBus>(new MessageBus(connectionString));

        return services;
    }
}
