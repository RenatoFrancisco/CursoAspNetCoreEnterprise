namespace NSE.WebApp.MVC.Extensions;

public class PaginationViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IPagedList pagedModel) => View();
}
