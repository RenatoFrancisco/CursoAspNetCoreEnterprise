namespace NSE.Orders.Domain;

public class Voucher : Entity, IAggregateRoot
{
    public string Code { get; private set; }
    public decimal? Percent { get; private set; }
    public decimal? DiscountValue { get; private set; }
    public int Amount { get; private set; }
    public DiscountType DiscountType { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? UsedOn { get; private set; }
    public DateTime ExpireDate { get; private set; }
    public bool Active { get; private set; }
    public bool Used { get; private set; }

    public bool IsValidForUtilization() =>
        new ActiveVoucherSpecification()
            .And(new DateVoucherSpecification())
            .And(new AmountVoucherSpecification())
            .IsSatisfiedBy(this);

    public void CheckAsUsed()
    {
        Active = false;
        Used = true;
        Amount = 0;
        UsedOn = DateTime.Now;
    }

    public void DebtAmount()
    {
        Amount -= 1;
        if (Amount >= 1) return;

        CheckAsUsed();
    }
}
