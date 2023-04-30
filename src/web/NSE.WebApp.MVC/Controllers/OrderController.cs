namespace NSE.WebApp.MVC.Controllers;

public class OrderController : MainController
{
    private readonly ICustomerService _customerService;
    private readonly IOrdersBffService _orderBffService;

    public OrderController(ICustomerService customerService, IOrdersBffService orderBffService)
    {
        _customerService = customerService;
        _orderBffService = orderBffService;
    }

    [HttpGet("delivery-address")]
    public async Task<IActionResult> DeliveryAddress()
    {
        var cart = await _orderBffService.GetCartAsync();
        if (!cart.Items.Any()) return RedirectToAction("Index", "Cart");

        var address = await _customerService.GetAddressAsync();
        var order = _orderBffService.MapToOrder(cart, address);

        return View(order);
    }

    [HttpGet("payment")]
    public async Task<IActionResult> Payment()
    {
        var cart = await _orderBffService.GetCartAsync();
        if (cart.Items.Count == 0) return RedirectToAction("Index", "Carrinho");

        var order = _orderBffService.MapToOrder(cart, null);

        return View(order);
    }

    [HttpPost("finish-order")]
    public async Task<IActionResult> FinishOrder(TransactionOrderViewModel orderTransaction)
    {
        if (!ModelState.IsValid) return View("Payment", _orderBffService.MapToOrder(
            await _orderBffService.GetCartAsync(), null));

        var response = await _orderBffService.FinishOrderAsync(orderTransaction);

        if (ResponseHasErrors(response))
        {
            var cart = await _orderBffService.GetCartAsync();
            if (cart.Items.Count == 0) return RedirectToAction("Index", "Cart");

            var pedidoMap = _orderBffService.MapToOrder(cart, null);
            return View("Payment", pedidoMap);
        }

        return RedirectToAction("OrderConcluded");
    }

    [HttpGet("concluded-order")]
    public async Task<IActionResult> ConcludedOrder() => View("ConfirmationOrder", await _orderBffService.GetLastOrderAsync());

    [HttpGet("my-orders")]
    public async Task<IActionResult> MeusPedidos() => View(await _orderBffService.GetListByCustomerIdAsync());
}
