namespace NSE.Cliente.API.Data;

public class CustomersContext : DbContext, IUnitOfWork
{
    public CustomersContext(DbContextOptions<CustomersContext> options) : base(options) 
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        ChangeTracker.AutoDetectChangesEnabled= true;
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Adresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            x => x.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomersContext).Assembly);
    }

    public async Task<bool> CommitAsync() => await base.SaveChangesAsync() > 0;
}
