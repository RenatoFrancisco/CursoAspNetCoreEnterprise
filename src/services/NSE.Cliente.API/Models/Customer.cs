﻿namespace NSE.Cliente.API.Models;

public class Customer : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Cpf Cpf { get; private set; }
    public bool Excluded { get; private set; }
    public Address Address { get; private set; }

    protected Customer() { }

    public Customer(Guid id, string name, string email, string cpf)
    {
        Id = id;
        Name = name;
        Email = new Email(email);
        Cpf = new Cpf(cpf);
        Excluded = false;
    }

    public void ChangeEmail(string email) => Email = new Email(email);

    public void SetAddress(Address address) => Address = address;
}
