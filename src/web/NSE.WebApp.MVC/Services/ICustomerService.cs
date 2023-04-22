namespace NSE.WebApp.MVC.Services
{
    public interface ICustomerService
    {
        Task<AddressViewModel> GetAddressAsync();
        Task<ResponseResult> AddAddressAsync(AddressViewModel address);
    }
}
