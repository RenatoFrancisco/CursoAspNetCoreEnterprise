namespace NSE.WebApp.MVC.Services;

public class CustomerService : Service, ICustomerService
{
    private readonly HttpClient _httpClient;

    public CustomerService(HttpClient httpClient, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CustomerUrl);
    }

    public async Task<AddressViewModel> GetAddressAsync()
    {
        var response = await _httpClient.GetAsync("/customer/address/");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        HandleResponseErrors(response);

        return await DeserializeResponseObject<AddressViewModel>(response);
    }

    public async Task<ResponseResult> AddAddressAsync(AddressViewModel address)
    {
        var addressContent = GetContent(address);

        var response = await _httpClient.PostAsync("/customer/address/", addressContent);

        if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

        return ReturnsOk();
    }
}
