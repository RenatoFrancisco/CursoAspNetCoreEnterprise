namespace NSE.Bff.Compras.Services;

public interface ICatalogoService { }

public class CatalogService : Service, ICatalogoService
{
    private readonly HttpClient _httpClient;

    public CatalogService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CatalogUrl);
    }
}
