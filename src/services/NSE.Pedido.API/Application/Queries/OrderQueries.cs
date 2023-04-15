namespace NSE.Pedido.API.Application.Queries;


public interface IOrderQueries
{
    Task<OrderDTO> GetLastorderAsync(Guid customerId);
    Task<IEnumerable<OrderDTO>> GetListByCustomerIdAsync(Guid customerId);
}

public class OrderQueries : IOrderQueries
{
    private readonly IOrderRepository _orderRepository;

    public OrderQueries(IOrderRepository orderRepository) => _orderRepository = orderRepository;

    public async Task<OrderDTO> GetLastorderAsync(Guid customerId)
    {
        const string sql = @"SELECT
                                O.ID AS 'ProductId', O.CODE, O.USEDVOUCHER, O.DISCOUNT, O.TOTALVALUE,O.STATUSORDER,
                                O.STREET,O.NUMBER, O.NEIGHBORHOOD, O.ZIPCODE, O.COMPLEMENT, O.CITY, O.STATE,
                                IO.ID AS 'ProdutoItemId',IO.PRODUCTNAME, IO.AMOUNT, IO.PRODUCTIMAGE, IO.UNITVALUE 
                                FROM ORDERS O
                                INNER JOIN ITEMSORDER IO ON O.ID = IO.ORDERID 
                                WHERE O.CUSTOMERID = @customerId 
                                AND O.CREATEDON between DATEADD(minute, -3,  GETDATE()) and DATEADD(minute, 0,  GETDATE())
                                AND O.STATUSPEDIDO = 1 
                                ORDER BY O.CREATEDON DESC";

        var order = await _orderRepository
            .GetConnection()
            .QueryAsync<dynamic>(sql, new { customerId });

        return MapOrder(order);
    }

    public async Task<IEnumerable<OrderDTO>> GetListByCustomerIdAsync(Guid customerId)
    {
        var orders = await _orderRepository.GetListByCustomerId(customerId);

        return orders.Select(OrderDTO.ParaPedidoDTO);
    }

    private OrderDTO MapOrder(dynamic result)
    {
        var order = new OrderDTO
        {
            Code = result[0].CODE,
            Status = result[0].STATUSORDER,
            TotalValue = result[0].TOTALVALUE,
            Discount = result[0].DISCOUNT,
            UsedVoucher = result[0].USEDVOUCHER,

            ItemOrders = new List<ItemOrderDTO>(),
            Address = new AddressDTO
            {
                Street = result[0].STREET,
                Neighborhood = result[0].NEIGHBORHOOD,
                ZipCode = result[0].ZIPCODE,
                City = result[0].CITY,
                Complement = result[0].COMPLEMENT,
                State = result[0].STATE,
                Number = result[0].NUMBER
            }
        };

        foreach (var item in result)
        {
            var itemOrder = new ItemOrderDTO
            {
                Name = item.PRODUCTNAME,
                Value = item.UNITVALUE,
                Amount = item.AMOUNT,
                Image = item.PRODUCTIMAGE
            };

            order.ItemOrders.Add(itemOrder);
        }

        return order;
    }
}

