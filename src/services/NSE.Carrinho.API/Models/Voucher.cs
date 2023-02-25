namespace NSE.Carrinho.API.Model
{
    public class Voucher
    {
        public decimal? Percentual { get; set; }
        public decimal? DiscountedValue { get; set; }
        public string Code { get; set; }
        public VoucherDiscountType DiscountType { get; set; }
    }

    public enum VoucherDiscountType
    {
        Percetage = 0,
        Value = 1
    }
}