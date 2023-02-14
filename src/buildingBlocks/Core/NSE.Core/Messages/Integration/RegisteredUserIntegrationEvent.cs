﻿namespace NSE.Core.Messages.Integration;

public class RegisteredUserIntegrationEvent
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Cpf { get; private set; }

    public RegisteredUserIntegrationEvent(Guid id, string name, string email, string cpf)
    {
        Id = id;
        Name = name;
        Email = email;
        Cpf = cpf;
    }
}
