namespace NSE.WebApp.MVC.Extensions;

public static class PollyExtensions
{
    public static AsyncRetryPolicy<HttpResponseMessage> WaitAndTry() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
            }, (outcome, timespan, retryCount, context) =>
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Attempt: {retryCount}");
                Console.ForegroundColor = ConsoleColor.White;
            });
}
