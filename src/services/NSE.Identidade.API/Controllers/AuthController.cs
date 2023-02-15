



using EasyNetQ;

namespace NSE.Identidade.API.Controllers;

[Route("api/identity")]
public class AuthController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppSettings _appSettings;

    private IBus _bus;

    public AuthController(SignInManager<IdentityUser> signInManager,
                          UserManager<IdentityUser> userManager,
                          IOptions<AppSettings> appSettings,
                          IBus bus)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _appSettings = appSettings.Value;
        _bus = bus;
    }

    [HttpPost("new-account")]
    public async Task<ActionResult> Register(RegisterUser registerUser) 
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        IdentityUser user = new()
        {
            UserName = registerUser.Email,
            Email =  registerUser.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, registerUser.Password);
        if (result.Succeeded)
        {
            var success = await RegisterCustomerAsync(registerUser);
            return CustomResponse(await GenerateJwt(user.Email));
        }

        result.Errors
            .ToList()
            .ForEach(e => AddError(e.Description));

        return CustomResponse();
    }

    private async Task<ResponseMessage> RegisterCustomerAsync(RegisterUser registerUser)
    {
        var user = await _userManager.FindByEmailAsync(registerUser.Email);

        var registeredUser = new RegisteredUserIntegrationEvent(
            Guid.Parse(user.Id), registerUser.Name, registerUser.Email, registerUser.Cpf);

        _bus = RabbitHutch.CreateBus("host=localhost:5672");

        var success = await _bus.Rpc.RequestAsync<RegisteredUserIntegrationEvent, ResponseMessage>(registeredUser);

        return success;
    }

    [HttpPost("authenticate")]

    public async Task<ActionResult> Login(LoginUser loginUser) 
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result =  await _signInManager.PasswordSignInAsync(loginUser.Email,
                                                               loginUser.Password,
                                                               isPersistent: false,
                                                               lockoutOnFailure: true);

        if (result.Succeeded) 
            return CustomResponse(await GenerateJwt(loginUser.Email));

        if (result.IsLockedOut)
        {
            AddError("User blocked temporarly due invalid attempts");
            return CustomResponse();
        }

        AddError("User or password incorrect");

        return CustomResponse();
    }

    private async Task<LoginResponseUser> GenerateJwt(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var claims = await _userManager.GetClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.Now).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.Now).ToString(), ClaimValueTypes.Integer64));

        foreach (var role in userRoles)
            claims.Add(new Claim("role", role));

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _appSettings.Issuer,
            Audience = _appSettings.ValidAt,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationInHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        var encodedToken = tokenHandler.WriteToken(token);

        var response = new LoginResponseUser
        {
            AccessToken = encodedToken,
            ExpireIn = TimeSpan.FromHours(_appSettings.ExpirationInHours).TotalSeconds,
            UserToken = new UserToken
            {
                Id = user.Id,
                Email = user.Email,
                Claims = claims.Select(c => new UserClaim { Type = c.Type, Value = c.Value })
            }
        };

        return response;
    }

    private static long ToUnixEpochDate(DateTime date)
        => ((long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds));
}