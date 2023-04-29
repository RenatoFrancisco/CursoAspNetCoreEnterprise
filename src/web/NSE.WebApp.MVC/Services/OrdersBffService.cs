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

    public async Task<ResponseResult> ApplyCartVoucherAsync(string voucher)
    {
        var itemContent = GetContent(voucher);
        var response = await _httpClient.PostAsync("/orders/cart/apply-voucher", itemContent);

        if (!HandleResponseErrors(response))
            return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOk();
    }

    public TransactionOrderViewModel MapToOrder(CartViewModel cart, AddressViewModel address)
    {
        var order = new TransactionOrderViewModel
        {
            TotalValue = cart.TotalValue,
            Items = cart.Items,
            Discount = cart.Discount,
            UsedVoucher = cart.UsedVoucher,
            VoucherCode = cart.Voucher?.Code
        };

        if (address is not null)
        {
            order.Address = new AddressViewModel
            {
                Street = address.Street,
                Number = address.Number,
                Neighborhood = address.Neighborhood,
                ZipCode = address.ZipCode,
                Complement = address.Complement,
                City = address.City,
                State = address.State
            };
        }

        return order;
    }

    public async Task<ResponseResult> FinishOrderAsync(TransactionOrderViewModel transactionOrder)
    {
        var orderContent = GetContent(transactionOrder);

        var response = await _httpClient.PostAsync("/orders/finish/", orderContent);

        if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOk();
    }

    public async Task<OrderViewModel> GetLastOrderAsync()
    {
        var response = await _httpClient.GetAsync("/orders/last/");

        HandleResponseErrors(response);

        return await DeserializeResponseObject<OrderViewModel>(response);
    }

    public async Task<IEnumerable<OrderViewModel>> GetListByCustomerIdAsync()
    {
        var response = await _httpClient.GetAsync("/orders/customer-list/");

        HandleResponseErrors(response);

        return await DeserializeResponseObject<IEnumerable<OrderViewModel>>(response);
    }
}
