using Caelum.Stella.CSharp.Http;


namespace NSE.Cliente.API.Application.Commands;

public class CustomerCommandHandler : CommandHandler, 
    IRequestHandler<RegisterCustomerCommand, ValidationResult>,
    IRequestHandler<AddAddressCommand, ValidationResult>
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerCommandHandler(ICustomerRepository customerRepository) => _customerRepository = customerRepository;

    public async Task<ValidationResult> Handle(RegisterCustomerCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid()) return message.ValidationResult;

        var customer = new Customer(message.Id, message.Name, message.Email, message.Cpf);

        var customerExists = await _customerRepository.FindByCpfAsync(message.Cpf);
        if (customerExists is not null) 
        {
            AddError("This cpf is already in use");
            return ValidationResult;
        }

        _customerRepository.Add(customer);

        customer.AddEvent(new RegisteredCustomerEvent(message.Id, message.Name, message.Email, message.Cpf));

        return await PersistDataAsync(_customerRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(AddAddressCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid()) return message.ValidationResult;

        var address = new Address(message.Street, message.Number, message.Complement, message.Neighborhood, 
                                  message.ZipCode, message.City, message.State, message.CustomerId);

        _customerRepository.AddAddress(address);

        return await PersistDataAsync(_customerRepository.UnitOfWork);
    }
}
