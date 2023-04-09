namespace NSE.Pedido.API.Application.DTO;

public class ItemOrderDTO
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public decimal Value { get; set; }
    public string Image { get; set; }
    public int Amount { get; set; }

    public static ItemOrder ParaPedidoItem(ItemOrderDTO itemOrderDTO)
    {
        return new ItemOrder(itemOrderDTO.ProductId, itemOrderDTO.Name, itemOrderDTO.Amount,
            itemOrderDTO.Value, itemOrderDTO.Image);
    }
}
