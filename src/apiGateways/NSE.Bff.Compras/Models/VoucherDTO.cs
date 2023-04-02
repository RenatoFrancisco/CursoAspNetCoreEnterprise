namespace NSE.Bff.Compras.Models;

public class VoucherDTO
{
    public decimal? Percent { get; set; }
    public decimal? DiscountValue { get; set; }
    public int Amount { get; set; }
    public int DiscountType { get; set; }
}
