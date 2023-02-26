namespace NSE.WebAPI.Core.User;

public class AspNetUser : IAspNetUser
{
    private readonly IHttpContextAccessor _accessor;

    public AspNetUser(IHttpContextAccessor accessor) => _accessor = accessor;

    public string Name => _accessor.HttpContext.User.Identity.Name;

    public Guid GetUserId() =>
        IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;

    public string GetUserEmail() =>
        IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : string.Empty;

    public string GetUserToken() => IsAuthenticated() ? _accessor.HttpContext.User.GetUserToken() : string.Empty;

    public bool IsAuthenticated() => _accessor.HttpContext.User.Identity.IsAuthenticated;

    public bool HasRole(string role) => _accessor.HttpContext.User.IsInRole(role);

    public IEnumerable<Claim> GetClaims() => _accessor.HttpContext.User.Claims;

    public HttpContext GetHttpContext() => _accessor.HttpContext;
}
