namespace NSE.WebApp.MVC.Models;

public class TokenUser
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<ClaimUser> Claims { get; set; }
}