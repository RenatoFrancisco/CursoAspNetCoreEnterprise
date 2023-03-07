namespace NSE.WebApp.MVC.Services;

public class CartService : Service, ICartService
{
    private readonly HttpClient _httpClient;

    public CartService(HttpClient httpClient, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CartUrl);
    }

    public async Task<CartViewModel> GetAsync()
    {
        var response = await _httpClient.GetAsync("/cart/");
        HandleResponseErrors(response);
        return await DeserializeResponseObject<CartViewModel>(response);
    }

    public async Task<ResponseResult> AddItemCartAsync(ItemProductViewModel product)
    {
        var itemContent = GetContent(product);
        var response = await _httpClient.PostAsync("/cart/", itemContent);

        if (!HandleResponseErrors(response)) 
            return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOk();
    }

    public async Task<ResponseResult> UpdateItemCartAsync(Guid productId, ItemProductViewModel product)
    {
        var itemContent = GetContent(product);
        var response = await _httpClient.PutAsync($"/cart/{productId}", itemContent);

        if (!HandleResponseErrors(response))
            return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOk();
    }

    public async Task<ResponseResult> RemoveItemCartAsync(Guid productId)
    {
        var response = await _httpClient.DeleteAsync($"/cart/{productId}");

        if (!HandleResponseErrors(response))
            return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOk();
    }
}
