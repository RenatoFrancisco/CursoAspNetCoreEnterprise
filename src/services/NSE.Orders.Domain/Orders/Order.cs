namespace NSE.Orders.Domain.Orders;

public class Order : Entity, IAggregateRoot
{
    public Order(Guid customerId,
                 decimal totalValue,
                 List<ItemOrder> itemsOrder,
                 bool usedVoucher = false,
                 decimal discount = 0,
                 Guid? voucherId = null)
    {
        CustomerId = customerId;
        TotalValue = totalValue;
        _itemsOrder = itemsOrder;

        Discount = discount;
        UsedVoucher = usedVoucher;
        VoucherId = voucherId;
    }

    // EF ctor
    protected Order() { }

    public int Code { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid? VoucherId { get; private set; }
    public bool UsedVoucher { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalValue { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public StatusOrder StatusOrder { get; private set; }

    private readonly List<ItemOrder> _itemsOrder;
    public IReadOnlyCollection<ItemOrder> ItemsOrder => _itemsOrder;

    public Address Address { get; private set; }

    // EF Rel.
    public Voucher Voucher { get; private set; }

    public void AuthorizeOrder() => StatusOrder = StatusOrder.Authorized;

    public void CancelOrder() => StatusOrder = StatusOrder.Cancelled;

    public void FinishOrder() => StatusOrder = StatusOrder.Paid;

    public void SetVoucher(Voucher voucher)
    {
        UsedVoucher = true;
        VoucherId = voucher.Id;
        Voucher = voucher;
    }

    public void SetAddress(Address address) => Address = address;

    public void CalculateOrderValue()
    {
        TotalValue = ItemsOrder.Sum(p => p.CalculateValue());
        CalculateDiscountTotalValue();
    }

    public void CalculateDiscountTotalValue()
    {
        if (!UsedVoucher) return;

        decimal discount = 0;
        var value = TotalValue;

        if (Voucher.DiscountType == DiscountType.Percent)
        {
            if (Voucher.Percent.HasValue)
            {
                discount = (value * Voucher.Percent.Value) / 100;
                value -= discount;
            }
        }
        else
        {
            if (Voucher.DiscountValue.HasValue)
            {
                discount = Voucher.DiscountValue.Value;
                value -= discount;
            }
        }

        TotalValue = value < 0 ? 0 : value;
        Discount = discount;
    }
}
