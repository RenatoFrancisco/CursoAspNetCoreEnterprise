namespace NSE.Bff.Compras.Services;

public interface ICartService 
{
    Task<CartDTO> Get();
    Task<ResponseResult> AddItemCart(ItemCartDTO product);
    Task<ResponseResult> UpdateItemCart(Guid productId, ItemCartDTO cart);
    Task<ResponseResult> RemoveItemCart(Guid productId);
}

public class CartService : Service, ICartService
{
    private readonly HttpClient _httpClient;

    public CartService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CartUrl);
    }

    public async Task<CartDTO> Get()
    {
        var response = await _httpClient.GetAsync("/cart/");

        HandleResponseErrors(response);

        return await DeserializeResponseObject<CartDTO>(response);
    }

    public async Task<ResponseResult> AddItemCart(ItemCartDTO product)
    {
        var itemContent = GetContent(product);

        var response = await _httpClient.PostAsync("/cart/", itemContent);

        if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOK();
    }

    public async Task<ResponseResult> UpdateItemCart(Guid productId, ItemCartDTO cart)
    {
        var itemContent = GetContent(cart);

        var response = await _httpClient.PutAsync($"/cart/{cart.ProductId}", itemContent);

        if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOK();
    }

    public async Task<ResponseResult> RemoveItemCart(Guid productId)
    {
        var response = await _httpClient.DeleteAsync($"/cart/{productId}");

        if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOK();
    }
}
