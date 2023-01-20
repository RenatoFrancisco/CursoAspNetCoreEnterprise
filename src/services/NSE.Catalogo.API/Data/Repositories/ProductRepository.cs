using NSE.Core.Data;

namespace NSE.Catalogo.API.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly CatalogContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public ProductRepository(CatalogContext context) => _context = context;

    public async Task<IEnumerable<Product>> GetAllAsync() => 
        await _context.Products.AsNoTrackingWithIdentityResolution().ToArrayAsync();

    public async Task<Product> GetAsync(Guid id) => await _context.Products.FindAsync(id);

    public void Add(Product product) => _context.Products.Add(product);

    public void Update(Product product) => _context.Products.Update(product);

    public void Dispose() => _context?.Dispose();
}
