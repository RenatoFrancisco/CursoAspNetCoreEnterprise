namespace NSE.WebApp.MVC.Services;

public interface IAuthenticationService
{
    Task<LoginResponseUser> Login(LoginUser loginUser);
    Task<LoginResponseUser> Register(RegisterUser registerUser);
}