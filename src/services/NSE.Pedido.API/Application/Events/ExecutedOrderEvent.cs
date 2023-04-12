namespace NSE.Pedido.API.Application.Events;

public class ExecutedOrderEvent : Event
{
    public Guid OrderId { get; private set; }
    public Guid CustomerId { get; private set; }

    public ExecutedOrderEvent(Guid orderId, Guid customerId)
    {
        OrderId = orderId;
        CustomerId = customerId;
    }
}
