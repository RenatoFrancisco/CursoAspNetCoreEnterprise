namespace NSE.Pedido.API.Application.DTO;

public class VoucherDTO
{
    public string Code { get; set; }
    public decimal? Percent { get; set; }
    public decimal? DiscountValue { get; set; }
    public int DiscountType { get; set; }
}
