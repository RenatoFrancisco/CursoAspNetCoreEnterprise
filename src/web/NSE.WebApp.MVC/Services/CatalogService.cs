namespace NSE.WebApp.MVC.Services;

public class CatalogService : Service, ICatalogService
{
    private readonly HttpClient _httpclient;

    public CatalogService(HttpClient httpclient, IOptions<AppSettings> settings)
    {
        httpclient.BaseAddress = new Uri(settings.Value.CatalogUrl);
        _httpclient = httpclient;
    }

    public async Task<PageViewModel<ProductViewModel>> GetAllAsync(int pageSize, int pageIndex, string query = null)
    {
        var response = await _httpclient.GetAsync($"catalog/products?ps={pageSize}&page={pageIndex}&q={query}");
        HandleResponseErrors(response);

        return await DeserializeResponseObject<PageViewModel<ProductViewModel>>(response);
    }

    public async Task<ProductViewModel> GetByIdAsync(Guid id)
    {
        var response = await _httpclient.GetAsync($"catalog/products/{id}");
        HandleResponseErrors(response);

        return await DeserializeResponseObject<ProductViewModel>(response);
    }
}
