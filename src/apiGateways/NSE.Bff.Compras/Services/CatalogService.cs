namespace NSE.Bff.Compras.Services;

public interface ICatalogService 
{
    Task<ItemProductDTO> GetProductAsync(Guid id);
}

public class CatalogService : Service, ICatalogService
{
    private readonly HttpClient _httpClient;

    public CatalogService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CatalogUrl);
    }

    public async Task<ItemProductDTO> GetProductAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"/catalog/products/{id}");
        HandleResponseErrors(response);

        return await DeserializeResponseObject<ItemProductDTO>(response);
    }
}
