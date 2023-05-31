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


    private void SetSubscribers()
    {
        _bus.SubscribeAsync<CancelledOrderIntegrationEvent>("CancelledOrder", async request =>
        await CancelPayment(request));

        _bus.SubscribeAsync<RegisteredOrderStockIntegrationEvent>("RegisteredOrderStock", async request =>
        await CapturarPagamento(request));
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetResponder();
        SetSubscribers();
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

    private async Task CancelPayment(CancelledOrderIntegrationEvent message)
    {
        using var scope = _serviceProvider.CreateScope();
        var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

        var response = await paymentService.CancelPayment(message.OrderId);

        if (!response.ValidationResult.IsValid)
            throw new DomainException($"Occured an error while cancelling the payment of the order {message.OrderId}");
    }

    private async Task CapturarPagamento(RegisteredOrderStockIntegrationEvent message)
    {
        using var scope = _serviceProvider.CreateScope();
        var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

        var response = await paymentService.CapturePaymentAsync(message.OrderId);

        if (!response.ValidationResult.IsValid)
            throw new DomainException($"Occured an error while capturing the payment of the order {message.OrderId}");

        await _bus.PublishAsync(new PaidOrderIntegrationEvent(message.CustomerId, message.OrderId));
    }

}
