namespace NSE.Bff.Compras.Models;

public class CartDTO
{
    public decimal TotalValue { get; set; }
    public decimal Discount { get; set; }
    public List<ItemCartDTO> Items { get; set; } = new List<ItemCartDTO>();
}
