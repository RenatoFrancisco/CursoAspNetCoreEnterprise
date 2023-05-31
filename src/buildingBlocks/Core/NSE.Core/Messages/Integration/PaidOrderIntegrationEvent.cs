namespace NSE.Core.Messages.Integration;

public class PaidOrderIntegrationEvent : IntegrationEvent
{
    public Guid CustomerId { get; private set; }
    public Guid OrderId { get; private set; }

    public PaidOrderIntegrationEvent(Guid customerId, Guid orderId)
    {
        CustomerId = customerId;
        OrderId = orderId;
    }
}
