namespace NSE.Cliente.API.Services;

public class CustomerRegisterIntegrationHandler : BackgroundService
{
    private IBus _bus;

    private readonly IServiceProvider _serviceProvider;

    public CustomerRegisterIntegrationHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus = RabbitHutch.CreateBus("host=localhost:5672");

        _bus.Rpc.RespondAsync<RegisteredUserIntegrationEvent, ResponseMessage>(async request => 
            new ResponseMessage(await RegisterCustomer(request)));

        return Task.CompletedTask;
    }

    private async Task<ValidationResult> RegisterCustomer(RegisteredUserIntegrationEvent message)
    {
        var customerCommand = new RegisterCustomerCommand(message.Id, message.Name, message.Email, message.Cpf);

        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
        var success = await mediator.SendCommand(customerCommand);

        return success;
    }
}
