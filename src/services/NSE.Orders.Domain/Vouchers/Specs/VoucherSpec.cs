namespace NSE.Pedidos.Domain.Specs;

public class DateVoucherSpecification : Specification<Voucher>
{
    public override Expression<Func<Voucher, bool>> ToExpression() => 
        voucher => voucher.ExpireDate >= DateTime.Now;
}

public class AmountVoucherSpecification : Specification<Voucher>
{
    public override Expression<Func<Voucher, bool>> ToExpression() => 
        voucher => voucher.Amount > 0;
}

public class ActiveVoucherSpecification : Specification<Voucher>
{
    public override Expression<Func<Voucher, bool>> ToExpression()
        => voucher => voucher.Active && !voucher.Used;
}