namespace NSE.Core.Messages.Integration;

public class ExecutedOrderIntegrationEvent : IntegrationEvent
{
    public Guid CustomerId { get; private set; }

    public ExecutedOrderIntegrationEvent(Guid customerId) => CustomerId = customerId;
}
