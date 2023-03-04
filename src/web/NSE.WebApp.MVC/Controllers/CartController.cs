namespace NSE.WebApp.MVC.Controllers;

public class CartController : MainController
{
    [Route("cart")]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpPost]
    [Route("cart/add-item")]
    public async Task<IActionResult> AddItemCart(ItemProductViewModel itemProduct)
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("cart/update-item")]
    public async Task<IActionResult> UpdateItemCart(Guid productId, int amount)
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("cart/remove-item")]
    public async Task<IActionResult> RemoveItemCart(Guid productId)
    {
        return RedirectToAction("Index");
    }
}