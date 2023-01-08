using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

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
        await DoLogin(response);

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
        await DoLogin(response);
        
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        return RedirectToAction("Index", "Home");
    }

    public async Task DoLogin(LoginResponseUser response)
    {
        var token =  GetFormatedToken(response.AccessToken);

        var claims = new List<Claim>();
        claims.Add(new Claim("JWT", response.AccessToken));
        claims.AddRange(token.Claims);

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
            IsPersistent = true
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    private static JwtSecurityToken GetFormatedToken(string jwtToken) =>
        new JwtSecurityTokenHandler().ReadJwtToken(jwtToken) as JwtSecurityToken;
}