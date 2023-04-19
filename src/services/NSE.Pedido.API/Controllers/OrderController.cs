namespace NSE.Pedido.API.Controllers;

public class OrderController : MainController
{
    private readonly IMediatorHandler _mediator;
    private readonly IAspNetUser _user;
    private readonly IOrderQueries _ordersQueries;

    public OrderController(IMediatorHandler mediator,
                           IAspNetUser user,
                           IOrderQueries ordersQueries)
    {
        _mediator = mediator;
        _user = user;
        _ordersQueries = ordersQueries;
    }

    [HttpPost("order")]
    public async Task<IActionResult> AddOrder(AddOrderCommand order)
    {
        order.CustomerId = _user.GetUserId();
        return CustomResponse(await _mediator.SendCommand(order));
    }

    [HttpGet("order/last")]
    public async Task<IActionResult> LastOrder()
    {
        var order = await _ordersQueries.GetLastorderAsync(_user.GetUserId());

        return order is null ? NotFound() : CustomResponse(order);
    }

    [HttpGet("order/customer-list")]
    public async Task<IActionResult> ListByCustomer()
    {
        var orders = await _ordersQueries.GetListByCustomerIdAsync(_user.GetUserId());

        return orders is null ? NotFound() : CustomResponse(orders);
    }
}
