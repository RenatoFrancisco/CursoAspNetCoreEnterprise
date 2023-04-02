namespace NSE.Bff.Compras.Services;

public interface ICartService 
{
    Task<CartDTO> GetAsync();
    Task<ResponseResult> AddItemCartAsync(ItemCartDTO product);
    Task<ResponseResult> UpdateItemCartAsync(Guid productId, ItemCartDTO cart);
    Task<ResponseResult> RemoveItemCartAsync(Guid productId);
    Task<ResponseResult> ApplyCartVoucherAsync(VoucherDTO voucher);
}

public class CartService : Service, ICartService
{
    private readonly HttpClient _httpClient;

    public CartService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CartUrl);
    }

    public async Task<CartDTO> GetAsync()
    {
        var response = await _httpClient.GetAsync("/cart/");

        HandleResponseErrors(response);

        return await DeserializeResponseObject<CartDTO>(response);
    }

    public async Task<ResponseResult> AddItemCartAsync(ItemCartDTO product)
    {
        var itemContent = GetContent(product);

        var response = await _httpClient.PostAsync("/cart/", itemContent);

        if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOK();
    }

    public async Task<ResponseResult> UpdateItemCartAsync(Guid productId, ItemCartDTO cart)
    {
        var itemContent = GetContent(cart);

        var response = await _httpClient.PutAsync($"/cart/{cart.ProductId}", itemContent);

        if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOK();
    }

    public async Task<ResponseResult> RemoveItemCartAsync(Guid productId)
    {
        var response = await _httpClient.DeleteAsync($"/cart/{productId}");

        if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOK();
    }

    public async Task<ResponseResult> ApplyCartVoucherAsync(VoucherDTO voucher)
    {
        var itemContent = GetContent(voucher);

        var response = await _httpClient.PostAsJsonAsync("/cart/apply-voucher/", itemContent);

        if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOK();
    }
}
