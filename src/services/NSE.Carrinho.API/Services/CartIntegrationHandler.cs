using NSE.Core.Messages.Integration;

namespace NSE.Carrinho.API.Services;

public class CartIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public CartIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetSubscribers();
        return Task.CompletedTask;
    }

    private void SetSubscribers() =>
        _bus.SubscribeAsync<ExecutedOrderIntegrationEvent>("ExecutedOrder", EraseCart);

    private async Task EraseCart(ExecutedOrderIntegrationEvent message)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CartContext>();

        var cart = await context.CustomerCart.FirstOrDefaultAsync(c => c.CustomerId == message.CustomerId);
        if (cart is not null) 
        {
            context.CustomerCart.Remove(cart);
            await context.SaveChangesAsync();
        }
    }
}
