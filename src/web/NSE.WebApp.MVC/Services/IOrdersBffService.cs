namespace NSE.WebApp.MVC.Services;

public interface IOrdersBffService
{
    Task<CartViewModel> GetCartAsync();
    Task<int> GetAmountCartAsync();
    Task<ResponseResult> AddItemCartAsync(ItemCartViewModel product);
    Task<ResponseResult> UpdateItemCartAsync(Guid productId, ItemCartViewModel product);
    Task<ResponseResult> RemoveItemCartAsync(Guid productId);
    Task<ResponseResult> ApplyCartVoucher(string voucher);
}
