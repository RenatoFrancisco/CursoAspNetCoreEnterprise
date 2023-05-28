namespace NSE.Pedido.API.Services;

public class OrderOrchestratorIntegrationHandler : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrderOrchestratorIntegrationHandler> _logger;
    private Timer _timer;

    public OrderOrchestratorIntegrationHandler(IServiceProvider serviceProvider,
                                               ILogger<OrderOrchestratorIntegrationHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Order Service has been started.");

        _timer = new Timer(ProcessOrders, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(15));

        return Task.CompletedTask;
    }

    private void ProcessOrders(object state)
    {
        _logger.LogInformation("Processing orders...");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Order Service has been finished.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose() => _timer?.Dispose();
}
