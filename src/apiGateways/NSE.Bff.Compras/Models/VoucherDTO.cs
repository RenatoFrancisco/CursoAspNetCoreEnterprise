namespace NSE.Bff.Compras.Models;

public class VoucherDTO
{
    public decimal? Percent { get; set; }
    public decimal? DiscountValue { get; set; }
    public string Code { get; set; }
    public int DiscountType { get; set; }
}
