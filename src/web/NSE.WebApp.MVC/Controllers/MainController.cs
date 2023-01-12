namespace NSE.WebApp.MVC.Controllers;

public abstract class MainController : Controller
{
    protected bool ResponseHasErrors(ResponseResult response)
    {
        if (response is not null && response.Errors.Messages.Any()) 
        {
            foreach (var message in response.Errors.Messages)
                ModelState.AddModelError(string.Empty, message);

            return true;
        }
        

        return false;
    }
}