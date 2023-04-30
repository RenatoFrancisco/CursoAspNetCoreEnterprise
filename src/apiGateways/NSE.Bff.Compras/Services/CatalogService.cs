using NSE.Bff.Compras.Models;

namespace NSE.Bff.Compras.Services;

public interface ICatalogService 
{
    Task<ItemProductDTO> GetProductAsync(Guid id);
    Task<IEnumerable<ItemProductDTO>> GetItemsAsync(IEnumerable<Guid> ids);
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

    public async Task<IEnumerable<ItemProductDTO>> GetItemsAsync(IEnumerable<Guid> ids)
    {
        var idsRequest = string.Join(",", ids);

        var response = await _httpClient.GetAsync($"/catalog/products/list/{idsRequest}/");

        HandleResponseErrors(response);

        return await DeserializeResponseObject<IEnumerable<ItemProductDTO>>(response);
    }
}
