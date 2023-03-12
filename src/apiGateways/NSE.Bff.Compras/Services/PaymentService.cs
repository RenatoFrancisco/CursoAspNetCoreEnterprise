namespace NSE.Bff.Compras.Services;

public interface IPagamentoService { }

public class PaymentService : Service, IPagamentoService
{
    private readonly HttpClient _httpClient;

    public PaymentService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.PaymentUrl);
    }
}