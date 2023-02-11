namespace NSE.Cliente.API.Application.Events;

public class CustomerEventHandler : INotificationHandler<RegisteredCustomerEvent>
{
    public Task Handle(RegisteredCustomerEvent notification, CancellationToken cancellationToken) =>
        Task.CompletedTask;
}
