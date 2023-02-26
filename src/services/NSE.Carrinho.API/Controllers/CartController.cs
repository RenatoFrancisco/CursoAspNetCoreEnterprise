namespace NSE.Carrinho.API.Controllers;

[Authorize]
public class CartController : MainController
{
    private readonly IAspNetUser _user;

    public CartController(IAspNetUser user)
    {
        _user = user;
    }

    [HttpGet("cart")]
    public async Task<CustomerCart> GetCart()
    {
        _user.GetUserId();
        return null;
    }

    [HttpPost("cart")]
    public async Task<IActionResult> AddItemCart()
    {
        return CustomResponse();
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
}
