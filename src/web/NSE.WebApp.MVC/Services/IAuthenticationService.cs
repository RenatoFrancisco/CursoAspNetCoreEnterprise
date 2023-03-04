namespace NSE.WebApp.MVC.Services;

public interface IAuthenticationService
{
    Task<LoginResponseUser> LoginAsync(LoginUser loginUser);
    Task<LoginResponseUser> RegisterAsync(RegisterUser registerUser);
}