namespace NSE.WebApp.MVC.Services;

public interface ICatalogService
{
    Task<IEnumerable<ProductViewModel>> GetAllAsync();

    Task<ProductViewModel> GetByIdAsync(Guid id);
}
