namespace NSE.Cliente.API.Application.Commands;

public class RegisterCustomerCommand : Command
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Cpf { get; private set; }

    public RegisterCustomerCommand(Guid id, string nome, string email, string cpf)
    {
        AggregateId = id;
        Id = id;
        Nome = nome;
        Email = email;
        Cpf = cpf;
    }
}
