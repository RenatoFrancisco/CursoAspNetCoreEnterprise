using Microsoft.AspNetCore.Mvc;
using NSE.Identidade.API.Models;

namespace NSE.Identidade.API.Controllers;

[Route("api/identity")]
public class AuthController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("new-account")]
    public async Task<ActionResult> Register(RegisterUser registerUser) 
    {
        if (!ModelState.IsValid) return BadRequest();

        IdentityUser user = new()
        {
            UserName = registerUser.Email,
            Email =  registerUser.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, registerUser.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok();
        }

        return BadRequest();
    }

    [HttpPost("auth")]

    public async Task<ActionResult> Login(LoginUser loginUser) 
    {
        if (!ModelState.IsValid) return BadRequest();

        var result =  await _signInManager.PasswordSignInAsync(loginUser.Email,
                                                               loginUser.Password,
                                                               isPersistent: false,
                                                               lockoutOnFailure: true);

        if (result.Succeeded) return Ok();

        return BadRequest();
    }
}