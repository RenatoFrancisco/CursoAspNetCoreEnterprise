namespace NSE.Pagamento.API.Services;

public class PaymentIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public PaymentIntegrationHandler(
                        IServiceProvider serviceProvider,
                        IMessageBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }

    private void SetResponder() =>
        _bus.RespondAsync<InitializedOrderIntegrationEvent, ResponseMessage>(AutorizarPagamento);

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetResponder();
        return Task.CompletedTask;
    }

    private async Task<ResponseMessage> AutorizarPagamento(InitializedOrderIntegrationEvent message)
    {
        using var scope = _serviceProvider.CreateScope();
        var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
        var payment = new Payment
        {
            OrderId = message.OrderId,
            PaymentType = (PaymentType)message.PaymentType,
            Value = message.Value,
            CreditCard = new CreditCard(
                message.CardName, message.CardNumber, message.ExpireMonthYear, message.CVV)
        };

        var response = await paymentService.AuthorizePaymentAsync(payment);

        return response;
    }
}
