namespace NSE.WebApp.MVC.Services;

public abstract class Service
{
    // protected static bool HandleResponseErrors(HttpResponseMessage response) => (int)response.StatusCode switch
    // {
    //     401 or 403 or 404 or 500 => throw new CustomHttpResponseException(response.StatusCode),
    //     var s when s >= 200 && s < 300 => true,
    //     _ => false
    // };

    protected bool HandleResponseErrors(HttpResponseMessage response)
    {
        switch ((int)response.StatusCode)
        {
            case 401:
            case 403:
            case 404:
            case 500:
                throw new CustomHttpResponseException(response.StatusCode);

            case 400:
                return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }
}