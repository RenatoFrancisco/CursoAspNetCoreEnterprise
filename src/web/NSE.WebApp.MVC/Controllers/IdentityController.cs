namespace NSE.WebApp.MVC.Controllers;

public class IdentityController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public IdentityController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

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
        var response = await _authenticationService.Register(registerUser);
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
        var response = await _authenticationService.Login(loginUser);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        return RedirectToAction("Index", "Home");
    }
}