namespace NSE.Pedido.API.Application.Commands;

public class OrderCommandHandler : CommandHandler, IRequestHandler<AddOrderCommand, ValidationResult>
{
    public Task<ValidationResult> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
