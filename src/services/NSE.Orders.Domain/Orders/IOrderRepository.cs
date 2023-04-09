namespace NSE.Orders.Domain.Orders;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order> GetById(Guid id);
    Task<IEnumerable<Order>> GetListByCustomerId(Guid customerId);
    void Add(Order order);
    void Update(Order order);

    DbConnection GetConnection();


    /* Item Order */
    Task<ItemOrder> GetItemById(Guid id);
    Task<ItemOrder> GetItemByOrder(Guid orderId, Guid productId);
}
