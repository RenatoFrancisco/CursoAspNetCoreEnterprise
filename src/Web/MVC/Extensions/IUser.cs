using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace NSE.WebApp.MVC.Extensions
{
    public interface IUser
    {
         string Name { get; }
         Guid ObterUserId();
         string ObterUserEmail();
         bool EstaAutenticado();
         string ObterUserToken();
         bool PossuiRole(string role);
         IEnumerable<Claim> ObterClaims();
         HttpContext ObterHttpContext();
    }

    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor) => _accessor = accessor;

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public Guid ObterUserId() =>
            EstaAutenticado() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;

        public string ObterUserEmail() =>
            EstaAutenticado() ? _accessor.HttpContext.User.GetUserEmail() : string.Empty;

        public string ObterUserToken() =>
            EstaAutenticado() ? _accessor.HttpContext.User.GetUserToken() : string.Empty;
        public bool EstaAutenticado() => _accessor.HttpContext.User.Identity.IsAuthenticated;

        public bool PossuiRole(string role) => _accessor.HttpContext.User.IsInRole(role);

        public IEnumerable<Claim> ObterClaims() => _accessor.HttpContext.User.Claims;

        public HttpContext ObterHttpContext() => _accessor.HttpContext;
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal is null) throw new ArgumentException(nameof(principal));

            var claim = principal.FindFirst("sub");
            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal is null) throw new ArgumentException(nameof(principal));

            var claim = principal.FindFirst("email");
            return claim?.Value;
        }

        public static string GetUserToken(this ClaimsPrincipal principal) 
        {
            if (principal is null) throw new ArgumentException(nameof(principal));

            var claim = principal.FindFirst("JWT");
            return claim?.Value;
        }
    }
}