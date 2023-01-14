namespace NSE.WebApp.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Route("error/{id:length(3,3)}")]
    public IActionResult Error(int id)
    {
        var modelErro = new ErrorViewModel();

        if (id == 500)
        {
            modelErro.Message = "An error occurred while processing your request.! Try again later or contact our support team.";
            modelErro.Title = "An error occurred !";
            modelErro.ErrorCode = id;
        }
        else if (id == 404)
        {
            modelErro.Message =
                "The page you are looking for does not exist!<br />If you need more details, contact our support team.";
            modelErro.Title = "Ops! Page not found.";
            modelErro.ErrorCode = id;
        }
        else if (id == 403)
        {
            modelErro.Message = "You do not have permision for doing that.";
            modelErro.Title = "Access denied!";
            modelErro.ErrorCode = id;
        }
        else
        {
            return StatusCode(404);
        }

        return View("Error", modelErro);
    }
}
