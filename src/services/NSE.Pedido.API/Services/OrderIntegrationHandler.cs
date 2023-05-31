namespace NSE.Pedido.API.Services;

public class OrderIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public OrderIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetSubscribers();
        return Task.CompletedTask;
    }

    private void SetSubscribers()
    {
        _bus.SubscribeAsync<CancelledOrderIntegrationEvent>("CancelledOrder", CancelOrder);

        _bus.SubscribeAsync<PaidOrderIntegrationEvent>("PaidOrder", FinalizarPedido);
    }

    private async Task CancelOrder(CancelledOrderIntegrationEvent message)
    {
        using var scope = _serviceProvider.CreateScope();
        var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

        var order = await orderRepository.GetById(message.OrderId);
        order.CancelOrder();

        orderRepository.Update(order);

        if (!await orderRepository.UnitOfWork.CommitAsync())
            throw new DomainException($"Occured an error while cancelling the order {message.OrderId}");
    }

    private async Task FinalizarPedido(PaidOrderIntegrationEvent message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

            var order = await orderRepository.GetById(message.OrderId);
            order.FinishOrder();

            orderRepository.Update(order);

            if (!await orderRepository.UnitOfWork.CommitAsync())
                throw new DomainException($"Occured an error while finishing the order {message.OrderId}");
        }
    }
}
