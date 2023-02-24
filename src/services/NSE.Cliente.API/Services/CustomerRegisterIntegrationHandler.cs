namespace NSE.Cliente.API.Services;

public class CustomerRegisterIntegrationHandler : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageBus _bus;

    public CustomerRegisterIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetResponder();
        return Task.CompletedTask;
    }

    private void SetResponder()
    {
        _bus.RespondAsync<RegisteredUserIntegrationEvent, ResponseMessage>(async request => await RegisterCustomer(request));
        _bus.AdvancedBus.Connected += OnConnect;
    }

    private void OnConnect(object s, ConnectedEventArgs e) => SetResponder();


    private async Task<ResponseMessage> RegisterCustomer(RegisteredUserIntegrationEvent message)
    {
        var customerCommand = new RegisterCustomerCommand(message.Id, message.Name, message.Email, message.Cpf);

        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
        var success = await mediator.SendCommand(customerCommand);

        return new ResponseMessage(success);
    }
}
