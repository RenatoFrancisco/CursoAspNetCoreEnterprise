using Microsoft.Extensions.DependencyInjection;
using NSE.Catalogo.API.Data.Repositories;
using NSE.Catalogo.API.Models;

namespace NSE.Catalogo.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
        }
    }
}