namespace NSE.Identidade.API.Models;

public class LoginResponseUser
{
    public string AccessToken { get; set; }

    public double ExpireIn { get; set; }

    public UserToken UserToken { get; set; }
}