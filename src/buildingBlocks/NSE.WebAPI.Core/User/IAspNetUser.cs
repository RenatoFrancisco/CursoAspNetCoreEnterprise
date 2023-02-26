namespace NSE.WebAPI.Core.User;

public interface IAspNetUser
{
    string Name { get; }

    Guid GetUserId();

    string GetUserEmail();

    string GetUserToken();

    bool IsAuthenticated();

    bool HasRole(string role);

    IEnumerable<Claim> GetClaims();

    HttpContext GetHttpContext();
}