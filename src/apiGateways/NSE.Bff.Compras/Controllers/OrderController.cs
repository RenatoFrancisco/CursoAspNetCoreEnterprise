namespace NSE.Bff.Compras.Controllers;

public class OrderController : MainController
{
    private readonly ICatalogService _catalogService;
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    private readonly ICustomerService _customerService;

    public OrderController(ICatalogService catalogService,
                           ICartService cartService,
                           IOrderService orderService,
                           ICustomerService customerService)
    {
        _catalogService = catalogService;
        _cartService = cartService;
        _orderService = orderService;
        _customerService = customerService;
    }

    [HttpPost]
    [Route("buys/order")]
    public async Task<IActionResult> AddOrder(OrderDTO order)
    {
        var cart = await _cartService.GetAsync();
        var products = await _catalogService.GetItemsAsync(cart.Items.Select(p => p.ProductId));
        var address = await _customerService.GetAddressAsync();

        if (!await ValidateCartProducts(cart, products)) return CustomResponse();

        PopulateOrderData(cart, address, order);

        return CustomResponse(await _orderService.FinishOrderAsync(order));
    }

    [HttpGet("buys/order/last")]
    public async Task<IActionResult> LastOrder()
    {
        var order = await _orderService.GetLastOrderAsync();
        if (order is null)
        {
            AddError("Order not found!");
            return CustomResponse();
        }

        return CustomResponse(order);
    }

    [HttpGet("buys/order/customer-list")]
    public async Task<IActionResult> ListByCustomer()
    {
        var orders = await _orderService.GetListByCustomerIdAsync();

        return orders is null ? NotFound() : CustomResponse(orders);
    }

    private async Task<bool> ValidateCartProducts(CartDTO cart, IEnumerable<ItemProductDTO> products)
    {
        if (cart.Items.Count != products.Count())
        {
            var availableItems = cart.Items.Select(c => c.ProductId).Except(products.Select(p => p.Id)).ToList();

            foreach (var itemId in availableItems)
            {
                var itemCart = cart.Items.FirstOrDefault(c => c.ProductId == itemId);
                AddError($"The {itemCart.Name} item is no longer available in the catalog, please remove it from the cart to proceed with the purchase.");
            }

            return false;
        }

        foreach (var itemCart in cart.Items)
        {
            var catalogProduct = products.FirstOrDefault(p => p.Id == itemCart.ProductId);

            if (catalogProduct.Value != itemCart.Value)
            {
                var msgErro = $"O product {itemCart.Name} has change the value (from: " +
                              $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", itemCart.Value)} to: " +
                              $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", catalogProduct.Value)}) since was added in the cart.";

                AddError(msgErro);

                var removeResponse = await _cartService.RemoveItemCartAsync(itemCart.ProductId);
                if (ResponseHasErrors(removeResponse))
                {
                    AddError($"It was not possible to automatically remove the {itemCart.Name} product from your cart, please remove and add it again if you still wish to purchase this item.");
                    return false;
                }

                itemCart.Value = catalogProduct.Value;
                var addResponse = await _cartService.AddItemCartAsync(itemCart);

                if (ResponseHasErrors(addResponse))
                {
                    AddError($"It was not possible to automatically update the {itemCart.Name} product from your cart, please remove and add it again if you still wish to purchase this item.");
                    return false;
                }

                ClearErrors();
                AddError(msgErro + " We have updated the value in your cart, please check your order and, if you prefer, remove the product.");

                return false;
            }
        }

        return true;
    }

    private void PopulateOrderData(CartDTO cart, AddressDTO address, OrderDTO order)
    {
        order.VoucherCode = cart.Voucher?.Code;
        order.UsedVoucher = cart.UsedVoucher;
        order.TotalValue = cart.TotalValue;
        order.Discount = cart.Discount;
        order.Items = cart.Items;

        order.Address = address;
    }
}
