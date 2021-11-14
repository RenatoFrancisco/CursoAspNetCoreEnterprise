using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly HttpClient _httpClient;

        public AutenticacaoService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            var loginContent = new StringContent(JsonSerializer.Serialize(usuarioLogin),
                                                 Encoding.UTF8,
                                                 "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5000/api/identidade/autenticar", loginContent);
            var result = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return  JsonSerializer.Deserialize<UsuarioRespostaLogin>(result, options);
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
        {
            var registroContent = new StringContent(JsonSerializer.Serialize(usuarioRegistro),
                                                 Encoding.UTF8,
                                                 "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5000/api/identidade/nova-conta", registroContent);
            var result = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return  JsonSerializer.Deserialize<UsuarioRespostaLogin>(result, options);
        }
    }
}