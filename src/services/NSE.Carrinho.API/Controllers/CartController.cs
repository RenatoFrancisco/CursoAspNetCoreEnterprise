namespace NSE.Carrinho.API.Controllers;

[Authorize]
public class CartController : MainController
{
    private readonly IAspNetUser _user;
    private readonly CartContext _context;

    public CartController(IAspNetUser user, CartContext context)
    {
        _user = user;
        _context = context;
    }

    [HttpGet("cart")]
    public async Task<CustomerCart> GetCart() => await GetCustomerCartAsync() ?? new CustomerCart();

    [HttpPost("cart")]
    public async Task<IActionResult> AddItemCart(ItemCart item)
    {
        var cart = await GetCustomerCartAsync();
        if (cart is null)
            HandleNewCart(item);
        else
            HandleExistentCart(cart, item);

        if (!IsValidOperation) return CustomResponse();

        var result = await _context.SaveChangesAsync();
        if (result <= 0) AddError("Ocurred an error while persisting into database");

        return CustomResponse();
    }

    private void HandleNewCart(ItemCart item)
    {
        var cart = new CustomerCart(_user.GetUserId());
        cart.AddItem(item);

        _context.CustomerCart.Add(cart);
    }

    private void HandleExistentCart(CustomerCart cart, ItemCart item)
    {
        var existentProduct = cart.ExistentItemCart(item);

        cart.AddItem(item);

        if (existentProduct)
            _context.ItemsCart.Update(cart.GetByProductId(item.ProductId));
        else
            _context.ItemsCart.Add(item);

        _context.CustomerCart.Update(cart);
    }

    [HttpPut("cart/{productId:guid}")]
    public async Task<IActionResult> UpdateItemCart(Guid productId, ItemCart item)
    {
        return CustomResponse();
    }

    [HttpPut("cart/{productId:guid}")]
    public async Task<IActionResult> RemoveItemCart(Guid productId)
    {
        return CustomResponse();
    }

    private async Task<CustomerCart> GetCustomerCartAsync() =>
        await _context.CustomerCart
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.CustomerId == _user.GetUserId());
}
