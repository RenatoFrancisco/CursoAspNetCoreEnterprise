namespace NSE.Carrinho.API.Model;

public class Voucher
{
    public decimal? Percentage { get; set; }
    public decimal? DiscountedValue { get; set; }
    public string Code { get; set; }
    public VoucherDiscountType DiscountType { get; set; }
}

public enum VoucherDiscountType
{
    Percentage = 0,
    Value = 1
}