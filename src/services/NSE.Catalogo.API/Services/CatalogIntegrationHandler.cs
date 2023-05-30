namespace NSE.Catalogo.API.Services;

public class CatalogIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public CatalogIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetSubscribers();
        return Task.CompletedTask;
    }

    private void SetSubscribers() =>
        _bus.SubscribeAsync<AuthorizedOrderIntegrationEvent>("AuthorizedOrder", RegisteredStock);

    private async Task RegisteredStock(AuthorizedOrderIntegrationEvent message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var productsWithStock = new List<Product>();
            var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

            var idsProducts = string.Join(",", message.Items.Select(c => c.Key));
            var products = await productRepository.GetProductsById(idsProducts);

            if (products.Count != message.Items.Count)
            {
                CancelOrderWithoutStock(message);
                return;
            }

            foreach (var product in products)
            {
                var productAmount = message.Items.FirstOrDefault(p => p.Key == product.Id).Value;

                if (product.IsAvailable(productAmount))
                {
                    product.RemoveFromStock(productAmount);
                    productsWithStock.Add(product);
                }
            }

            if (productsWithStock.Count != message.Items.Count)
            {
                CancelOrderWithoutStock(message);
                return;
            }

            foreach (var product in productsWithStock)
            {
                productRepository.Update(product);
            }

            if (!await productRepository.UnitOfWork.CommitAsync())
            {
                throw new DomainException($"Ocured a problem while updating order's stock {message.OrderId}");
            }

            var registeredOrder = new RegisteredOrderStockIntegrationEvent(message.CustomerId, message.OrderId);
            await _bus.PublishAsync(registeredOrder);
        }
    }

    public async void CancelOrderWithoutStock(AuthorizedOrderIntegrationEvent message)
    {
        var pedidoCancelado = new CancelledOrderIntegrationEvent(message.CustomerId, message.OrderId);
        await _bus.PublishAsync(pedidoCancelado);
    }
}
