namespace NSE.WebAPI.Core.Identity;

public class CriteriaClaimFilter : IAuthorizationFilter
{
    private readonly Claim _claim;

    public CriteriaClaimFilter(Claim claim) => _claim = claim;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated) 
        {
            context.Result = new StatusCodeResult(401);
            return;
        }

        if (!CustomAuthorization.ValidateUserClaim(context.HttpContext, _claim.Type, _claim.Value))
            context.Result = new StatusCodeResult(403);
    }
}
