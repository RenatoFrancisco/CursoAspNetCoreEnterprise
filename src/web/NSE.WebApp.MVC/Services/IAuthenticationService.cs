namespace NSE.WebApp.MVC.Services;

public interface IAuthenticationService
{
    Task<string> Login(LoginUser loginUser);
    Task<string> Register(RegisterUser registerUser);
}