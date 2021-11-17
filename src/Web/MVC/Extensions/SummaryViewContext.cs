using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NSE.WebApp.MVC.Extensions
{
    public class SummaryViewContext : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync() => View();
    }
}