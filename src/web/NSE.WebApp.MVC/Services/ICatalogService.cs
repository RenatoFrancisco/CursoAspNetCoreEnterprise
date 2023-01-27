namespace NSE.WebApp.MVC.Services;

public interface ICatalogService
{
    Task<IEnumerable<ProductViewModel>> GetAll();

    Task<ProductViewModel> Get(Guid id);
}
