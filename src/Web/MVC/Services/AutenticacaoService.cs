using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;

        public AutenticacaoService(HttpClient httpClient,
                                   IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AutenticacaoUrl);
            _httpClient = httpClient;
        } 

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            var loginContent = ObterConteudo(usuarioLogin);

            var response = await _httpClient.PostAsync("/api/identidade/autenticar", loginContent);
            var result = await response.Content.ReadAsStringAsync();

            if (!TratarErrosResponse(response))
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjeto<ResponseResult>(response)
                };

            return await DeserializarObjeto<UsuarioRespostaLogin>(response);
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
        {
            var registroContent = ObterConteudo(usuarioRegistro);

            var response = await _httpClient.PostAsync("/api/identidade/nova-conta", registroContent);
            var result = await response.Content.ReadAsStringAsync();

            if (!TratarErrosResponse(response))
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjeto<ResponseResult>(response)
                };

            return await DeserializarObjeto<UsuarioRespostaLogin>(response);
        }
    }
}