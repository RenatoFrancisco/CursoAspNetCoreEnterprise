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

        await PersistAsync();
        return CustomResponse();
    }

    [HttpPut("cart/{productId:guid}")]
    public async Task<IActionResult> UpdateItemCart(Guid productId, ItemCart item)
    {
        var cart = await GetCustomerCartAsync();
        var itemCart = await GetValidatedItemCartAsync(productId, cart, item);
        if (itemCart is null) return CustomResponse();

        cart.UpdateUnits(itemCart, item.Amount);

        ValidateCart(cart);
        if (!IsValidOperation) return CustomResponse();

        _context.ItemsCart.Update(itemCart);
        _context.CustomerCart.Update(cart);

        await PersistAsync();
        return CustomResponse();
    }

    [HttpPut("cart/{productId:guid}")]
    public async Task<IActionResult> RemoveItemCart(Guid productId)
    {
        var cart = await GetCustomerCartAsync();
        var itemCart = await GetValidatedItemCartAsync(productId, cart);
        if (itemCart is null) return CustomResponse();

        cart.RemoveItem(itemCart);

        ValidateCart(cart);
        if (!IsValidOperation) return CustomResponse();

        _context.ItemsCart.Remove(itemCart);
        _context.CustomerCart.Update(cart);

        await PersistAsync();
        return CustomResponse();
    }

    private void HandleNewCart(ItemCart item)
    {
        var cart = new CustomerCart(_user.GetUserId());
        cart.AddItem(item);

        ValidateCart(cart);
        _context.CustomerCart.Add(cart);
    }

    private void HandleExistentCart(CustomerCart cart, ItemCart item)
    {
        var existentProduct = cart.ExistentItemCart(item);

        cart.AddItem(item);
        ValidateCart(cart);

        if (existentProduct)
            _context.ItemsCart.Update(cart.GetByProductId(item.ProductId));
        else
            _context.ItemsCart.Add(item);

        _context.CustomerCart.Update(cart);
    }

    private async Task<CustomerCart> GetCustomerCartAsync() =>
        await _context.CustomerCart
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.CustomerId == _user.GetUserId());

    private async Task<ItemCart> GetValidatedItemCartAsync(Guid productId, CustomerCart cart, ItemCart item = null)
    {
        if (item is not null && productId != item.ProductId)
        {
            AddError("The item does not correspond with what was supplied");
            return null;
        }

        if (cart is null)
        {
            AddError("Cart not found");
            return null;
        }

        var itemCart = await _context.ItemsCart
            .FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId ==  productId);

        if (itemCart is null || !cart.ExistentItemCart(itemCart)) 
        {
            AddError("The item is not in cart");
            return null;
        }

        return itemCart;
    }

    private async Task PersistAsync()
    {
        var result = await _context.SaveChangesAsync();
        if (result <= 0) AddError("Ocurred an error while persisting into database");
    }

    private bool ValidateCart(CustomerCart cart)
    {
        if (cart.IsValid()) return true;

        cart.ValidationResult.Errors.ToList()
            .ForEach(e => AddError(e.ErrorMessage));

        return false;
    }
}
