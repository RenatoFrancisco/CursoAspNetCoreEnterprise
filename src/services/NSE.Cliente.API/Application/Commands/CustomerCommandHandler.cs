namespace NSE.Cliente.API.Application.Commands;

public class CustomerCommandHandler : CommandHandler, IRequestHandler<RegisterCustomerCommand, ValidationResult>
{
    public void Handle(RegisterCustomerCommand message)
    {

    }

    public async Task<ValidationResult> Handle(RegisterCustomerCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid()) return message.ValidationResult;

        var customer = new Customer(message.Id, message.Name, message.Email, message.Cpf);

        return message.ValidationResult;
    }
}
