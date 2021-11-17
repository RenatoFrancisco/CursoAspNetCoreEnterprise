using System.Net.Http;
using System;
using NSE.WebApp.MVC.Extensions;

namespace NSE.WebApp.MVC.Services
{
    public abstract class Service
    {
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