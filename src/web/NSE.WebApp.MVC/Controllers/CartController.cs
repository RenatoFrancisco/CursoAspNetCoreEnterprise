namespace NSE.WebApp.MVC.Controllers;

public class CartController : MainController
{
    private readonly ICartService _cartService;
    private readonly ICatalogService _catalogService;

    public CartController(ICartService cartService, 
                          ICatalogService catalogService)
    {
        _cartService = cartService;
        _catalogService = catalogService;
    }

    [Route("cart")]
    public async Task<IActionResult> Index() => View(await _cartService.GetAsync());

    [HttpPost]
    [Route("cart/add-item")]
    public async Task<IActionResult> AddItemCart(ItemProductViewModel itemProduct)
    {
        var product = await _catalogService.GetByIdAsync(itemProduct.ProductId);
        ValidateItemCart(product, itemProduct.Amount);

        itemProduct.Name = product.Name;
        itemProduct.Value = product.Value;
        itemProduct.Image = product.Image;

        var response = await _cartService.AddItemCartAsync(itemProduct);
        if (ResponseHasErrors(response)) 
            return View("Index", await _cartService.GetAsync());

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("cart/update-item")]
    public async Task<IActionResult> UpdateItemCart(Guid productId, int amount)
    {
        var product = await _catalogService.GetByIdAsync(productId);
        ValidateItemCart(product, amount);

        if (!IsValidOperation())
            return View("Index", await _cartService.GetAsync());

        var itemProduto = new ItemProductViewModel { ProductId = productId, Amount= amount };

        var response = await _cartService.UpdateItemCartAsync(productId, itemProduto);
        if (ResponseHasErrors(response))
            return View("Index", await _cartService.GetAsync());

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("cart/remove-item")]
    public async Task<IActionResult> RemoveItemCart(Guid productId)
    {
        var product = await _catalogService.GetByIdAsync(productId);
        if (product is null)
        {
            AddErrorValidation("The product does not exist");
            return View("Index", await _cartService.GetAsync());
        }

        var response = await _cartService.RemoveItemCartAsync(productId);
        if (ResponseHasErrors(response))
            return View("Index", await _cartService.GetAsync());

        return RedirectToAction("Index");
    }

    private void ValidateItemCart(ProductViewModel product, int amount)
    {
        if (product is null)
            AddErrorValidation("The product does not exist");

        if (amount < 1)
            AddErrorValidation($"Choose at least 1 unit of the product {product.Name}");

        if (amount > product.AmountStock)
            AddErrorValidation($"The product {product.Name} has {product.AmountStock} unit(s) in stock, you have chosen {amount}");
    }
}