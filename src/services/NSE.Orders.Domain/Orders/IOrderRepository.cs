namespace NSE.Orders.Domain.Orders;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order> GetById(Guid id);
    Task<IEnumerable<Order>> GetListByCustomerId(Guid customerId);
    void Add(Order Order);
    void Update(Order Order);

    DbConnection GetConnection();


    /* Item Order */
    Task<ItemOrder> GetItemById(Guid id);
    Task<ItemOrder> GetItemByOrder(Guid orderId, Guid productId);
}
