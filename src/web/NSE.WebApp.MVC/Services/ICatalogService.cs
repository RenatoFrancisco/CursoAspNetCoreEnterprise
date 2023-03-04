namespace NSE.WebApp.MVC.Services;

public interface ICatalogService
{
    Task<IEnumerable<ProductViewModel>> GetAllAsync();

    Task<ProductViewModel> GetAsync(Guid id);
}
