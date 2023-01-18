using Microsoft.Extensions.Options;

namespace NSE.WebApp.MVC.Services;

public abstract class Service
{
    protected StringContent GetContent(object data) => 
        new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

    protected async Task<T> DeserializeResponseObject<T>(HttpResponseMessage responseMessage) =>
        JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStreamAsync(),
                                      new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    protected static bool HandleResponseErrors(HttpResponseMessage response) => (int)response.StatusCode switch
    {
        401 or 403 or 404 or 500 => throw new CustomHttpResponseException(response.StatusCode),
        var s when s >= 200 && s < 300 => true,
        _ => false
    };
}