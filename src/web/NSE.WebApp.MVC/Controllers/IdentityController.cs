namespace NSE.WebApp.MVC.Controllers;

public class IdentityController : Controller
{
    [HttpGet()]
    [Route("new-account")]
    public ActionResult Register()
    {
        return View();
    }

    [HttpPost()]
    [Route("new-account")]
    public async Task<ActionResult> Register(RegisterUser registerUser)
    {
        return View();
    }

    [HttpGet]
    [Route("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginUser loginUser)
    {
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        return RedirectToAction("Index", "Home");
    }
}