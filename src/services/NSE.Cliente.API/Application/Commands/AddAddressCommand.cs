namespace NSE.Cliente.API.Application.Commands;

public class AddAddressCommand : Command
{
    public Guid CustomerId { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string Complement { get; set; }
    public string Neighborhood { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public string State { get; set; }

    public AddAddressCommand() { }

    public AddAddressCommand(Guid customerId,
                             string street,
                             string number,
                             string complement,
                             string neighborhood,
                             string zipCode,
                             string city,
                             string state)
    {
        CustomerId = customerId;
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        ZipCode = zipCode;
        City = city;
        State = state;
    }

    public override bool IsValid()
    {
        ValidationResult = new AddressValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class AddressValidation : AbstractValidator<AddAddressCommand>
{
    public AddressValidation()
    {
        RuleFor(c => c.Street)
            .NotEmpty()
            .WithMessage("The Street is required");

        RuleFor(c => c.Number)
            .NotEmpty()
            .WithMessage("The Number is required");

        RuleFor(c => c.ZipCode)
            .NotEmpty()
            .WithMessage("The ZipCode is required");

        RuleFor(c => c.Neighborhood)
            .NotEmpty()
            .WithMessage("The Neighborhood is required");

        RuleFor(c => c.City)
            .NotEmpty()
            .WithMessage("The City is required");

        RuleFor(c => c.State)
            .NotEmpty()
            .WithMessage("The State is required");
    }
}
