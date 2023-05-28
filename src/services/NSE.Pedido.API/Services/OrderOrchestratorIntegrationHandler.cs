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

    private async void ProcessOrders(object state)
    {
        using var scope = _serviceProvider.CreateScope();

        var orderQueries = scope.ServiceProvider.GetRequiredService<IOrderQueries>();
        var order = await orderQueries.GetAuthorizedOrdersAsync();

        if (order is null) return;

        var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

        var authorizedOrder = new AuthorizedOrderIntegrationEvent(order.CustomerId, order.Id,
                    order.ItemOrders.ToDictionary(p => p.ProductId, p => p.Amount));

        await bus.PublishAsync(authorizedOrder);

        _logger.LogInformation($"Order ID: {order.Id} has been forward to be registered in stock.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Order Service has been finished.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose() => _timer?.Dispose();
}
