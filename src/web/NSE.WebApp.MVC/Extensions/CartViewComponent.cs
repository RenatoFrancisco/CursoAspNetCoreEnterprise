namespace NSE.WebApp.MVC.Extensions;

public class CartViewComponent : ViewComponent
{
    private readonly ICartService _cartService;

    public CartViewComponent(ICartService cartService) => _cartService = cartService;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var cart = await _cartService.GetAsync();
        return View(cart?.Items?.Count() ?? 0);    
    }
}
