using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Controllers;

public class CatalogController : MainController
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService) => _catalogService = catalogService;

    [HttpGet]
    [Route("")]
    [Route("catalog")]
    public async Task<IActionResult> Index()
    {
        var products = await _catalogService.GetAllAsync();
        return View(products);
    }


    [HttpGet]
    [Route("product-details/{id:guid}")]
    public async Task<IActionResult> ProductDetails(Guid id)
    {
        var product = await _catalogService.GetAsync(id);
        return View(product);
    }
}
