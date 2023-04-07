namespace NSE.WebApp.MVC.Controllers;

public class CartController : MainController
{
    private readonly IOrdersBffService _ordersBffService;

    public CartController(IOrdersBffService ordersBffService) => _ordersBffService = ordersBffService;

    [Route("cart")]
    public async Task<IActionResult> Index() => View(await _ordersBffService.GetCartAsync());

    [HttpPost]
    [Route("cart/add-item")]
    public async Task<IActionResult> AddItemCart(ItemCartViewModel itemProduct)
    {
        var response = await _ordersBffService.AddItemCartAsync(itemProduct);

        if (ResponseHasErrors(response)) 
            return View("Index", await _ordersBffService.GetCartAsync());

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("cart/update-item")]
    public async Task<IActionResult> UpdateItemCart(Guid productId, int amount)
    {
        var itemProduto = new ItemCartViewModel { ProductId = productId, Amount= amount };
        var response = await _ordersBffService.UpdateItemCartAsync(productId, itemProduto);

        if (ResponseHasErrors(response))
            return View("Index", await _ordersBffService.GetCartAsync());

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("cart/remove-item")]
    public async Task<IActionResult> RemoveItemCart(Guid productId)
    {
        var response = await _ordersBffService.RemoveItemCartAsync(productId);
        if (ResponseHasErrors(response))
            return View("Index", await _ordersBffService.GetCartAsync());

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("cart/apply-voucher")]
    public async Task<IActionResult> ApplyVoucher(string voucherCode)
    {
        var response = await _ordersBffService.ApplyCartVoucher(voucherCode);

        if (ResponseHasErrors(response))
            return View("Index", await _ordersBffService.GetCartAsync());

        return RedirectToAction("Index");
    }
}