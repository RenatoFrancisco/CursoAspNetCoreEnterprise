using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();
        }
    }
}