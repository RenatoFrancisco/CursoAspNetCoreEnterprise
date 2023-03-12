namespace NSE.Bff.Compras.Controllers;

public class CartController : MainController
{
    [HttpGet]
    [Route("orders/cart")]
    public async Task<IActionResult> Index()
    {
        return CustomResponse();
    }

    [HttpGet]
    [Route("orders/cart-amount")]
    public async Task<IActionResult> GetCartAmount()
    {
        return CustomResponse();
    }

    [HttpPost]
    [Route("orders/cart/items")]
    public async Task<IActionResult> AddItemCart()
    {
        return CustomResponse();
    }

    [HttpPut]
    [Route("orders/cart/items/{productId}")]
    public async Task<IActionResult> UpdateItemCart()
    {
        return CustomResponse();
    }

    [HttpDelete]
    [Route("orders/cart/items/{productId}")]
    public async Task<IActionResult> RemoveItemCart()
    {
        return CustomResponse();
    }
}
