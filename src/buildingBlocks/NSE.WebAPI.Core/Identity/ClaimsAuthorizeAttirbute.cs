namespace NSE.WebAPI.Core.Identity;

public class ClaimsAuthorizeAttirbute : TypeFilterAttribute
{
    public ClaimsAuthorizeAttirbute(string claimName, string claimValue) : base(typeof(CriteriaClaimFilter))
    {
        Arguments = new[] { new Claim(claimName, claimValue) };
    }
}
