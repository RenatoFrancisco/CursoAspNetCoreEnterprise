using NSE.Core.Data;

namespace NSE.Catalogo.API.Models;

public interface IProductRepository : IRepository<Product>
{
    Task<PagedResult<Product>> GetAllAsync(int pageSize, int pageIndex, string query = null);

    Task<Product> GetAsync(Guid id);

    Task<List<Product>> GetProductsById(string ids);

    void Add(Product product);

    void Update(Product product);
}
