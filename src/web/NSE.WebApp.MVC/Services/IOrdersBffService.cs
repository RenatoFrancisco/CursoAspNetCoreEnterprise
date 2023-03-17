namespace NSE.WebApp.MVC.Services;

public interface IOrdersBffService
{
    Task<CartViewModel> GetAsync();
    Task<int> GetAmountCartAsync();
    Task<ResponseResult> AddItemCartAsync(ItemCartViewModel product);
    Task<ResponseResult> UpdateItemCartAsync(Guid productId, ItemCartViewModel product);
    Task<ResponseResult> RemoveItemCartAsync(Guid productId);
}
