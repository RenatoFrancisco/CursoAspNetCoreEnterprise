namespace NSE.WebAPI.Core.Identity;

public class CustomAuthorization
{
    public static bool ValidateUserClaim(HttpContext context, string claimName, string claimValue) =>
        context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
}
