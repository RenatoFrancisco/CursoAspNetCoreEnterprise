namespace NSE.WebApp.MVC.Services;

public interface ICatalogService
{
    Task<PageViewModel<ProductViewModel>> GetAllAsync(int pageSize, int pageIndex, string query = null);

    Task<ProductViewModel> GetByIdAsync(Guid id);
}
