namespace NSE.Cliente.API.Controllers;

public class CustomersController : MainController
{
    private readonly IMediatorHandler _mediatorHandler;
    private readonly ICustomerRepository _customerRepository;
    private readonly IAspNetUser _user;

    public CustomersController(IMediatorHandler mediatorHandler,
                               ICustomerRepository customerRepository,
                               IAspNetUser user)
    {
        _mediatorHandler = mediatorHandler;
        _customerRepository = customerRepository;
        _user = user;
    }

    [HttpGet("Customers")]
    public async Task<IActionResult> Index() 
    {
        var validationResult = await _mediatorHandler.SendCommand(
            new RegisterCustomerCommand(Guid.NewGuid(), "Renato", "renato@teste.com", "78896511046"));

        return CustomResponse(validationResult);
    }

    [HttpGet("customer/address")]
    public async Task <IActionResult> GetAddress()
    {
        var address = await _customerRepository.GetAddressByIdAsync(_user.GetUserId());
        return address is null ? NotFound() : CustomResponse(address);
    }

    [HttpPost("customer/address")]
    public async Task<IActionResult> AddAddress(AddAddressCommand address)
    {
        address.CustomerId = _user.GetUserId();
        return CustomResponse(_mediatorHandler.SendCommand(address));
    }
}
