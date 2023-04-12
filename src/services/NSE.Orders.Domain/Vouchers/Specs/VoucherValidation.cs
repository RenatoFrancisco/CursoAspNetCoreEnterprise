namespace NSE.Orders.Domain.Vouchers.Specs;

public class VoucherValidation : SpecValidator<Voucher>
{
    public VoucherValidation()
    {
        var dateSpec = new DateVoucherSpecification();
        var amountSpec = new AmountVoucherSpecification();
        var activeSpec = new ActiveVoucherSpecification();

        Add("dateSpec", new Rule<Voucher>(dateSpec, "This voucher is expired"));
        Add("amountSpec", new Rule<Voucher>(amountSpec, "This voucher has already used"));
        Add("activeSpec", new Rule<Voucher>(activeSpec, "This voucher is not actived"));
    }
}
