using System.Net;
using System.Text;
using System.Text.Json;

namespace NSE.WebApp.MVC.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpclient;

    public AuthenticationService(HttpClient httpclient) => _httpclient = httpclient;

    public async Task<string> Login(LoginUser loginUser)
    {
        var loginContent = new StringContent(JsonSerializer.Serialize(loginUser), Encoding.UTF8, "application/json");

        var response = await _httpclient.PostAsync("https://localhost:7271/api/identity/authenticate", loginContent);

        return JsonSerializer.Deserialize<string>(await response.Content.ReadAsStreamAsync());
    }

    public async Task<string> Register(RegisterUser registerUser)
    {
        var registerContent = new StringContent(JsonSerializer.Serialize(registerUser), Encoding.UTF8, "application/json");

        var response = await _httpclient.PostAsync("https://localhost:7271/api/auth/new-account", registerContent);

        return JsonSerializer.Deserialize<string>(await response.Content.ReadAsStreamAsync());
    }
}