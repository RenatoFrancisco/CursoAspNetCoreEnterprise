namespace NSE.WebApp.MVC.Extensions;

public interface IUser
{
    string Name { get; }

    Guid GetUserId();

    string GetUserEmail();

    bool IsAuthenticated();

    bool HasRole(string role);

    IEnumerable<Claim> GetClaims();

    HttpContext GetHttpContext();
}