namespace NSE.Pedido.API.Application.Commands;

public class OrderCommandHandler : CommandHandler, IRequestHandler<AddOrderCommand, ValidationResult>
{
    private readonly IVoucherRepository _voucherRepository;
    private readonly IOrderRepository _orderRepository;

    public OrderCommandHandler(IVoucherRepository voucherRepository, 
                               IOrderRepository orderRepository)
    {
        _voucherRepository = voucherRepository;
        _orderRepository = orderRepository;
    }

    public async Task<ValidationResult> Handle(AddOrderCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid()) return message.ValidationResult;

        var order = MapOrder(message);

        if (!await ApplyVoucher(message, order)) return ValidationResult;

        if (!ValidateOrder(order)) return ValidationResult;

        order.AuthorizeOrder();

        order.AddEvent(new ExecutedOrderEvent(order.Id, order.CustomerId));

        _orderRepository.Add(order);

        return await PersistDataAsync(_orderRepository.UnitOfWork);
    }

    private Order MapOrder(AddOrderCommand message)
    {
        var address = new Address
        {
            Street = message.Addres.Street,
            Number = message.Addres.Number,
            Complement = message.Addres.Complement,
            Neighborhood = message.Addres.Neighborhood,
            ZipCode = message.Addres.ZipCode,
            City = message.Addres.City,
            State = message.Addres.State
        };

        var order = new Order(message.CustomerId, 
                               message.TotalValue,
                               message.ItemsOrder.Select(ItemOrderDTO.ToItemOrder).ToList(), 
                               message.UsedVoucher,
                               message.Discount);

        order.SetAddress(address);
        return order;
    }

    private async Task<bool> ApplyVoucher(AddOrderCommand message, Order order)
    {
        if (!message.UsedVoucher) return true;

        var voucher = await _voucherRepository.GetVoucherByCode(message.VoucherCode);
        if (voucher == null)
        {
            AddError("The provided voucher doesn't exist!");
            return false;
        }

        var voucherValidation = new VoucherValidation().Validate(voucher);
        if (!voucherValidation.IsValid)
        {
            voucherValidation.Errors.ToList().ForEach(m => AddError(m.ErrorMessage));
            return false;
        }

        order.SetVoucher(voucher);
        voucher.DebtAmount();

        _voucherRepository.Update(voucher);

        return true;
    }

    private bool ValidateOrder(Order order)
    {
        var originalOrderValue = order.TotalValue;
        var discountOrder = order.Discount;

        order.CalculateOrderValue();

        if (order.TotalValue != originalOrderValue)
        {
            AddError("The Order total value doesn't match with the order calculation");
            return false;
        }

        if (order.Discount != discountOrder)
        {
            AddError("The total value doesn't match with the order calculation");
            return false;
        }

        return true;
    }
}
