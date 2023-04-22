namespace NSE.Cliente.API.Data;

public class CustomersContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediator;

    public CustomersContext(DbContextOptions<CustomersContext> options, IMediatorHandler mediator) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        ChangeTracker.AutoDetectChangesEnabled = true;

        _mediator = mediator;
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<System.ComponentModel.DataAnnotations.ValidationResult>();
        modelBuilder.Ignore<Event>();

        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            x => x.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomersContext).Assembly);
    }

    public async Task<bool> CommitAsync()
    {
        var success = await base.SaveChangesAsync() > 0;
        if (success) await _mediator.PublishEventsAsync(this);

        return success;
    }
}
