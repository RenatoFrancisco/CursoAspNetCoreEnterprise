namespace NSE.Pedido.API.Application.DTO;

public class OrderDTO 
{
    public Guid Id { get; set; }
    public int Code { get; set; }

    public Guid CustomerId { get; set; }
    public int Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public decimal TotalValue { get; set; }

    public decimal Discount { get; set; }
    public string VoucherCode { get; set; }
    public bool UsedVoucher { get; set; }

    public List<ItemOrderDTO> ItemOrders { get; set; }
    public AddressDTO Address { get; set; }

    public static OrderDTO ParaPedidoDTO(Order order)
    {
        var ordeDTO = new OrderDTO
        {
            Id = order.Id,
            Code = order.Code,
            Status = (int)order.StatusOrder,
            CreatedOn = order.CreatedOn,
            TotalValue = order.TotalValue,
            Discount = order.Discount,
            UsedVoucher = order.UsedVoucher,
            ItemOrders = new List<ItemOrderDTO>(),
            Address = new AddressDTO()
        };

        foreach (var item in order.ItemsOrder)
        {
            ordeDTO.ItemOrders.Add(new ItemOrderDTO
            {
                Name = item.ProductName,
                Image = item.ProductImage,
                Amount = item.Amount,
                ProductId = item.ProductId,
                Value = item.UnitValue,
                OrderId = item.OrderId
            });
        }

        ordeDTO.Address = new AddressDTO
        {
            Street = order.Address.Street,
            Number = order.Address.Number,
            Complement = order.Address.Complement,
            Neighborhood = order.Address.Neighborhood,
            ZipCode = order.Address.ZipCode,
            City = order.Address.City,
            State = order.Address.State,
        };

        return ordeDTO;
    }
}
