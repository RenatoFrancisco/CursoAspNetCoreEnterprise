﻿namespace NSE.Cliente.API.Models;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer> FindByCpfAsync(string cpf);

    void Add(Customer customer);

    Task<Address> GetAddressByIdAsync(Guid id);
    void AddAddress(Address address);
}
