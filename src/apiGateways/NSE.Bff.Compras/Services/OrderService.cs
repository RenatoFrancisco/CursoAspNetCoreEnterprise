namespace NSE.Bff.Compras.Services;

public interface IPedidoService { }

public class OrderService : Service, IPedidoService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.OrderUrl);
    }
}