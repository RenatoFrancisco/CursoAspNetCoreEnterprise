namespace NSE.WebApp.MVC.Extensions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (CustomHttpResponseException ex)
        {
            HandleRequestExceptionAsync(httpContext, ex);
        }
    }

    private static void HandleRequestExceptionAsync(HttpContext context, CustomHttpResponseException httpResponseException)
    {
        if (httpResponseException.StatusCode == HttpStatusCode.Unauthorized)
        {
            context.Response.Redirect("/login");
            return;
        }

        context.Response.StatusCode = (int) httpResponseException.StatusCode;
    }
}