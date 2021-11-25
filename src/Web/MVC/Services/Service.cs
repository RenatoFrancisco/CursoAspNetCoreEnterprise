using System.Net.Http;
using System;
using NSE.WebApp.MVC.Extensions;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public abstract class Service
    {
        protected StringContent ObterConteudo(object dado) =>
            new StringContent(JsonSerializer.Serialize(dado),
                                                 Encoding.UTF8,
                                                 "application/json");

        protected async Task<T> DeserializarObjeto<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool TratarErrosResponse(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpResquestException(response.StatusCode);
                case 400:
                    return false; 
            }
            
            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}