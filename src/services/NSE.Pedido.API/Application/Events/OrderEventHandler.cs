namespace NSE.Pedido.API.Application.Events;

public class OrderEventHandler : INotificationHandler<ExecutedOrderEvent>
{
    private readonly IMessageBus _bus;

    public OrderEventHandler(IMessageBus bus) => _bus = bus;

    public async Task Handle(ExecutedOrderEvent message, CancellationToken cancellationToken) =>
        await _bus.PublishAsync(new ExecutedOrderIntegrationEvent(message.CustomerId));
}
