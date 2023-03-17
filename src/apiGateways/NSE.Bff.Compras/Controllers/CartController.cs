namespace NSE.Bff.Compras.Controllers;

public class CartController : MainController
{
    private readonly ICartService _cartService;
    private readonly ICatalogService _catalogService;

    public CartController(ICartService cartService, ICatalogService catalogService)
    {
        _cartService = cartService;
        _catalogService = catalogService;
    }

    [HttpGet]
    [Route("orders/cart")]
    public async Task<IActionResult> Index() => 
        CustomResponse(await _cartService.GetAsync());

    [HttpGet]
    [Route("orders/amount-cart")]
    public async Task<int> GetCartAmount()
    {
        var amount = await _cartService.GetAsync();
        return amount?.Items.Sum(x => x.Amount) ?? 0;
    }

    [HttpPost]
    [Route("orders/cart/items")]
    public async Task<IActionResult> AddItemCart(ItemCartDTO itemProduct)
    {
        var product = await _catalogService.GetProductAsync(itemProduct.ProductId);

        await ValidateItemCartAsync(product, itemProduct.Amount);
        if (!IsValidOperation) return CustomResponse();

        itemProduct.Name = product.Name;
        itemProduct.Value = product.Value;
        itemProduct.Image = product.Image;

        var response = await _cartService.AddItemCart(itemProduct);

        return CustomResponse(response);
    }

    [HttpPut]
    [Route("orders/cart/items/{productId}")]
    public async Task<IActionResult> UpdateItemCart(Guid productId, ItemCartDTO itemProduct)
    {
        var product = await _catalogService.GetProductAsync(productId);

        await ValidateItemCartAsync(product, itemProduct.Amount);
        if (!IsValidOperation) return CustomResponse();

        var response = await _cartService.UpdateItemCart(productId, itemProduct);

        return CustomResponse(response);
    }

    [HttpDelete]
    [Route("orders/cart/items/{productId}")]
    public async Task<IActionResult> RemoveItemCart(Guid productId)
    {
        var product = await _catalogService.GetProductAsync(productId);

        if (product is null)
        {
            AddError("The product does not exist!");
            return CustomResponse();
        }

        var response = await _cartService.RemoveItemCart(productId);

        return CustomResponse(response);
    }

    private async Task ValidateItemCartAsync(ItemProductDTO product, int amount, bool addProduct = false)
    {
        if (product == null) AddError("The product does not exist!");
        if (amount < 1) AddError($"Choose at least one {product.Name} product unit.");

        var cart = await _cartService.GetAsync();
        var itemCart = cart.Items.FirstOrDefault(p => p.ProductId == product.Id);

        if (itemCart is not null && addProduct && itemCart.Amount + amount > product.StockAmount)
        {
            AddError($"The {product.Name} product currently has {product.StockAmount} in stock, but you have selected {amount} units.");
            return;
        }

        if (amount > product.StockAmount)
            AddError($"The {product.Name} product currently has {product.StockAmount} in stock, but you have selected {amount} units.");
    }
}
