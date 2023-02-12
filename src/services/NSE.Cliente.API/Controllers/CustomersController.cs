namespace NSE.Cliente.API.Controllers;

public class CustomersController : MainController
{
    private readonly IMediatorHandler _mediatorHandler;

    public CustomersController(IMediatorHandler mediatorHandler)
    {
        _mediatorHandler = mediatorHandler;
    }

    [HttpGet("Customers")]
    public async Task<IActionResult> Index() 
    {
        var validationResult = await _mediatorHandler.SendCommand(
            new RegisterCustomerCommand(Guid.NewGuid(), "Renato", "renato@teste.com", "78896511046"));

        return CustomResponse(validationResult);
    }
}
