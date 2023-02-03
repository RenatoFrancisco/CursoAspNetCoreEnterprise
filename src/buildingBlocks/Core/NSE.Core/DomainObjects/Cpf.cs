namespace NSE.Core.DomainObjects;

public class Cpf
{
    public const int MAX_LENGTH = 11;

    public string Number { get; private set; }

    protected Cpf() { }

    public Cpf(string number)
    {
        if (!Validate(number)) throw new DomainException("Invalid CPF");

        Number = number;
    }

    public static bool Validate(string cpf) => new CPFValidator().IsValid(cpf);
}
