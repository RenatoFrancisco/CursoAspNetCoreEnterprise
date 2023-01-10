namespace NSE.WebApp.MVC.Controllers;

public abstract class MainController : Controller
{
    protected bool ResponseHasErrors(ResponseResult response)
    {
        if (response is not null && response.Errors.Messages.Any()) return true;

        return false;
    }
}