using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace NSE.Orders.Infra.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersContext _context;

    public OrderRepository(OrdersContext context) =>
        _context = context;

    public IUnitOfWork UnitOfWork => _context;

    public DbConnection GetConnection() => _context.Database.GetDbConnection();

    public async Task<Order> GetById(Guid id) => await _context.Orders.FindAsync(id);

    public async Task<IEnumerable<Order>> GetListByCustomerId(Guid customerId) =>
        await _context.Orders
                .Include(p => p.ItemsOrder)
                .AsNoTracking()
                .Where(p => p.CustomerId == customerId)
                .ToListAsync();

    public async Task<ItemOrder> GetItemById(Guid id) => await _context.ItemsOrder.FindAsync(id);

    public async Task<ItemOrder> GetItemByOrder(Guid orderId, Guid productId) =>
        await _context.ItemsOrder
                .FirstOrDefaultAsync(p => p.ProductId == productId && p.OrderId == orderId);

    public void Add(Order order) => _context.Orders.Add(order);

    public void Update(Order order) => _context.Orders.Update(order);

    public void Dispose() => _context?.Dispose();

}
