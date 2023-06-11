namespace NSE.WebApp.MVC.Controllers;

public class CatalogController : MainController
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService) => _catalogService = catalogService;

    [HttpGet]
    [Route("")]
    [Route("catalog")]
    public async Task<IActionResult> Index([FromQuery] int ps, [FromQuery] int page, [FromQuery] string q = null)
    {
        var products = await _catalogService.GetAllAsync(ps, page, q);
        ViewBag.Search = q;
        return View(products);
    }


    [HttpGet]
    [Route("product-details/{id:guid}")]
    public async Task<IActionResult> ProductDetails(Guid id)
    {
        var product = await _catalogService.GetByIdAsync(id);
        return View(product);
    }
}
