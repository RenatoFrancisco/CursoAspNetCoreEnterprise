namespace NSE.WebApp.MVC.Services;

public interface IOrdersBffService
{
    // Cart
    Task<CartViewModel> GetCartAsync();
    Task<int> GetAmountCartAsync();
    Task<ResponseResult> AddItemCartAsync(ItemCartViewModel product);
    Task<ResponseResult> UpdateItemCartAsync(Guid productId, ItemCartViewModel product);
    Task<ResponseResult> RemoveItemCartAsync(Guid productId);
    Task<ResponseResult> ApplyCartVoucherAsync(string voucher);
    TransactionOrderViewModel MapToOrder(CartViewModel cart, AddressViewModel address);

    // Order
    Task<ResponseResult> FinishOrderAsync(TransactionOrderViewModel transactionOrder);
    Task<OrderViewModel> GetLastOrderAsync();
    Task<IEnumerable<OrderViewModel>> GetListByCustomerIdAsync();
}
