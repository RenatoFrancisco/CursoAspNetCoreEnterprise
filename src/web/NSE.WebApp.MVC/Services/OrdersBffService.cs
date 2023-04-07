namespace NSE.WebApp.MVC.Services;

public class OrdersBffService : Service, IOrdersBffService
{
    private readonly HttpClient _httpClient;

    public OrdersBffService(HttpClient httpClient, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.OrdersBffUrl);
    }

    public async Task<CartViewModel> GetCartAsync()
    {
        var response = await _httpClient.GetAsync("/orders/cart/");
        HandleResponseErrors(response);
        return await DeserializeResponseObject<CartViewModel>(response);
    }

    public async Task<int> GetAmountCartAsync()
    {
        var response = await _httpClient.GetAsync("/orders/amount-cart/");
        HandleResponseErrors(response);
        return await DeserializeResponseObject<int>(response);
    }

    public async Task<ResponseResult> AddItemCartAsync(ItemCartViewModel product)
    {
        var itemContent = GetContent(product);
        var response = await _httpClient.PostAsync("/orders/cart/items/", itemContent);

        if (!HandleResponseErrors(response))
            return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOk();
    }

    public async Task<ResponseResult> UpdateItemCartAsync(Guid productId, ItemCartViewModel product)
    {
        var itemContent = GetContent(product);
        var response = await _httpClient.PutAsync($"/orders/cart/items/{productId}", itemContent);

        if (!HandleResponseErrors(response))
            return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOk();
    }

    public async Task<ResponseResult> RemoveItemCartAsync(Guid productId)
    {
        var response = await _httpClient.DeleteAsync($"/orders/cart/items/{productId}");

        if (!HandleResponseErrors(response))
            return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOk();
    }

    public async Task<ResponseResult> ApplyCartVoucher(string voucher)
    {
        var itemContent = GetContent(voucher);
        var response = await _httpClient.PostAsync("/orders/cart/apply-voucher", itemContent);

        if (!HandleResponseErrors(response))
            return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOk();
    }
}
