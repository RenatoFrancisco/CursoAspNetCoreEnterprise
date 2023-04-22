namespace NSE.Cliente.API.Models;

public class Address : Entity
{
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Complement { get; private set; }
    public string District { get; private set; }
    public string ZipCode { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }

    public Guid CustomerId { get; private set; }
    public Customer Customer { get; protected set; }


    public Address(string street, 
                   string number, 
                   string complement, 
                   string district, 
                   string zipCode, 
                   string city, 
                   string state,
                   Guid customerId)
    {
        Street = street;
        Number = number;
        Complement = complement;
        District = district;
        ZipCode = zipCode;
        City = city;
        State = state;
        CustomerId = customerId;
    }
}
