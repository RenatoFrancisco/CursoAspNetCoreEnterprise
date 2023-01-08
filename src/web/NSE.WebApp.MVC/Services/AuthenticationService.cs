using System.Net;
using System.Text;
using System.Text.Json;

namespace NSE.WebApp.MVC.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpclient;

    public AuthenticationService(HttpClient httpclient) => _httpclient = httpclient;

    public async Task<LoginResponseUser> Login(LoginUser loginUser)
    {
        var loginContent = new StringContent(JsonSerializer.Serialize(loginUser), Encoding.UTF8, "application/json");

        var response = await _httpclient.PostAsync("http://localhost:5226/api/identity/authenticate", loginContent);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<LoginResponseUser>(await response.Content.ReadAsStreamAsync(), options);
    }

    public async Task<LoginResponseUser> Register(RegisterUser registerUser)
    {
        var registerContent = new StringContent(JsonSerializer.Serialize(registerUser), Encoding.UTF8, "application/json");

        var response = await _httpclient.PostAsync("http://localhost:5226/api/auth/new-account", registerContent);

        return JsonSerializer.Deserialize<LoginResponseUser>(await response.Content.ReadAsStreamAsync());
    }
}