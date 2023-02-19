namespace NSE.Core.Utils;

public static class ConfigurationExtension
{
    public static string GetMessageQueueConnection(this IConfiguration configuration, string name) =>
        configuration?.GetSection("MessageQueueConnection")?[name];
}
