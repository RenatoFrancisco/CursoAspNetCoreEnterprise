namespace NSE.Cliente.API.Application.Commands.Validators;

public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
{
    public RegisterCustomerCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Invalid Customer's id");

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(c => c.Cpf)
            .Must(IsValidCpf)
            .WithMessage("Invalid Cpf");

        RuleFor(c => c.Email)
            .Must(IsValidEmail)
            .WithMessage("Invalid Cpf");
    }

    protected bool IsValidCpf(string cpf) => Cpf.Validate(cpf);

    private bool IsValidEmail(string email) => Email.Validate(email);
}
