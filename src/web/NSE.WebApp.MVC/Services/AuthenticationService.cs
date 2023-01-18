namespace NSE.WebApp.MVC.Services;

public class AuthenticationService : Service, IAuthenticationService
{
    private readonly HttpClient _httpclient;

    public AuthenticationService(HttpClient httpclient, IOptions<AppSettings> settings)
    {
        httpclient.BaseAddress = new Uri(settings.Value.AuthenticationUrl);
        _httpclient = httpclient;
    }

    public async Task<LoginResponseUser> Login(LoginUser loginUser)
    {
        var loginContent = GetContent(loginUser);

        var response = await _httpclient.PostAsync("/api/identity/authenticate", loginContent);
        if (!HandleResponseErrors(response))
        {
            return new LoginResponseUser
            {
                ResponseResult = await DeserializeResponseObject<ResponseResult>(response)
            };
        }

        return await DeserializeResponseObject<LoginResponseUser>(response);
    }

    public async Task<LoginResponseUser> Register(RegisterUser registerUser)
    {
        var registerContent = GetContent(registerUser);

        var response = await _httpclient.PostAsync("/api/identity/new-account", registerContent);
        if (!HandleResponseErrors(response))
        {
            return new LoginResponseUser
            {
                ResponseResult = await DeserializeResponseObject<ResponseResult>(response)
            };
        }

        return await DeserializeResponseObject<LoginResponseUser>(response);
    }
}