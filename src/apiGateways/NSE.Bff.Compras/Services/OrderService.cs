namespace NSE.Bff.Compras.Services;

public interface IOrderService 
{
    Task<VoucherDTO> GetVoucherByCodeAsync(string code);
    Task<ResponseResult> FinishOrderAsync(OrderDTO order);
    Task<OrderDTO> GetLastOrderAsync();
    Task<IEnumerable<OrderDTO>> GetListByCustomerIdAsync();
}

public class OrderService : Service, IOrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.OrderUrl);
    }

    public async Task<VoucherDTO> GetVoucherByCodeAsync(string code)
    {
        var response = await _httpClient.GetAsync($"/voucher/{code}");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        HandleResponseErrors(response);

        return await DeserializeResponseObject<VoucherDTO>(response);
    }

    public async Task<ResponseResult> FinishOrderAsync(OrderDTO order)
    {
        var orderContent = GetContent(order);

        var response = await _httpClient.PostAsync("/order/", orderContent);

        if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOK();
    }

    public async Task<OrderDTO> GetLastOrderAsync()
    {
        var response = await _httpClient.GetAsync("/order/last/");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        HandleResponseErrors(response);

        return await DeserializeResponseObject<OrderDTO>(response);
    }

    public async Task<IEnumerable<OrderDTO>> GetListByCustomerIdAsync()
    {
        var response = await _httpClient.GetAsync("/order/customer-list/");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        HandleResponseErrors(response);

        return await DeserializeResponseObject<IEnumerable<OrderDTO>>(response);
    }
}