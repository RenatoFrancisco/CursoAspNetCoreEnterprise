using Dapper;
using NSE.Catalogo.API.Models;

namespace NSE.Catalogo.API.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly CatalogContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public ProductRepository(CatalogContext context) => _context = context;

    public async Task<PagedResult<Product>> GetAllAsync(int pageSize, int pageIndex, string query = null)
    {
        var sql = @$"SELECT * FROM Products 
                      WHERE (@Name is null or Name LIKE '%' + @Name + '%') 
                      ORDER BY [Name] 
                      OFFSET {pageSize * (pageIndex - 1)} ROWS 
                      FETCH NEXT {pageSize} ROWS ONLY 
                      SELECT COUNT(Id) FROM Products 
                      WHERE (@Name is null or Name LIKE '%' + @Name + '%')";

        var multi = await _context.Database.GetDbConnection()
            .QueryMultipleAsync(sql, new { Name = query });

        var products = multi.Read<Product>();
        var total = multi.Read<int>().FirstOrDefault();

        return new PagedResult<Product>()
        {
            List = products,
            TotalResults = total,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Query = query
        };
    }

    public async Task<Product> GetAsync(Guid id) => await _context.Products.FindAsync(id);

    public async Task<List<Product>> GetProductsById(string ids)
    {
        var idsGuid = ids.Split(',')
                .Select(id => (Ok: Guid.TryParse(id, out var x), Value: x));

        if (!idsGuid.All(nid => nid.Ok)) return new List<Product>();

        var idsValue = idsGuid.Select(id => id.Value);

        return await _context.Products.AsNoTracking()
            .Where(p => idsValue.Contains(p.Id) && p.Active).ToListAsync();
    }

    public void Add(Product product) => _context.Products.Add(product);

    public void Update(Product product) => _context.Products.Update(product);

    public void Dispose() => _context?.Dispose();
}
