namespace NSE.Core.Messages.Integration;

public class InitializedOrderIntegrationEvent : IntegrationEvent
{
    public Guid CustomerId { get; set; }
    public Guid OrderId { get; set; }
    public int PaymentType { get; set; }
    public decimal Value { get; set; }

    public string CardName { get; set; }
    public string CardNumber { get; set; }
    public string ExpireMonthYear { get; set; }
    public string CVV { get; set; }
}
