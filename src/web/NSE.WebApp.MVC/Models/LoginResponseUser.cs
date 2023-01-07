namespace NSE.WebApp.MVC.Models;

public class LoginResponseUser
{
    public string AccessToken { get; set; }
    public double ExpiresIn { get; set; }
    public TokenUser TokenUser { get; set; }
}