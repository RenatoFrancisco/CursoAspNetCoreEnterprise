namespace NSE.Bff.Compras.Services;

public interface ICustomerService
{
    Task<AddressDTO> GetAddressAsync();
}

public class CustomerService : Service, ICustomerService
{
    private readonly HttpClient _httpClient;

    public CustomerService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CustomerUrl);
    }

    public async Task<AddressDTO> GetAddressAsync()
    {
        var response = await _httpClient.GetAsync("/customer/address/");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        HandleResponseErrors(response);

        return await DeserializeResponseObject<AddressDTO>(response);
    }
}
