namespace NSE.WebApp.MVC.Extensions;

public class CartViewComponent : ViewComponent
{
    private readonly IOrdersBffService _ordersBffService;

    public CartViewComponent(IOrdersBffService ordersBffService) => _ordersBffService = ordersBffService;

    public async Task<IViewComponentResult> InvokeAsync() => View(await _ordersBffService.GetAmountCartAsync());
}
