namespace NSE.Cliente.API.Data.Repositories;

public class CustomerRespository : ICustomerRepository
{
    private readonly CustomersContext _context;

    public CustomerRespository(CustomersContext context) => _context = context;

    public IUnitOfWork UnitOfWork => _context;


    public async Task<IEnumerable<Customer>> GetAllAsync() =>
        await _context.Customers.AsNoTrackingWithIdentityResolution().ToListAsync();

    public async Task<Customer> FindByCpfAsync(string cpf) =>
        await _context.Customers.FirstOrDefaultAsync(c => c.Cpf.Number == cpf);

    public void Add(Customer customer) => _context.Customers.Add(customer);

    public void Dispose() => _context?.Dispose();

    public async Task<Address> GetAddressByIdAsync(Guid id) =>
        await _context.Addresses.FirstOrDefaultAsync(address => address.CustomerId == id);

    public void AddAddress(Address address) => _context.Addresses.Add(address);
}
