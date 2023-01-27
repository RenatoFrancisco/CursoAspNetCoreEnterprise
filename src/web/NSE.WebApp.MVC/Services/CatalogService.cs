namespace NSE.WebApp.MVC.Services;

public class CatalogService : Service, ICatalogService
{
    private readonly HttpClient _httpclient;

    public CatalogService(HttpClient httpclient, IOptions<AppSettings> settings)
    {
        httpclient.BaseAddress = new Uri(settings.Value.CatalogUrl);
        _httpclient = httpclient;
    }

    public async Task<IEnumerable<ProductViewModel>> GetAll()
    {
        var response = await _httpclient.GetAsync("catalog/products/");
        HandleResponseErrors(response);

        return await DeserializeResponseObject<IEnumerable<ProductViewModel>>(response);
    }

    public async Task<ProductViewModel> Get(Guid id)
    {
        var response = await _httpclient.GetAsync($"catalog/products/{id}");
        HandleResponseErrors(response);

        return await DeserializeResponseObject<ProductViewModel>(response);
    }
}
