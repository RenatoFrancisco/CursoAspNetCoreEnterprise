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
}
