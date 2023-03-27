namespace NSE.Orders.Infra.Data;

public class OrdersContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediatorHandler;

    public OrdersContext(DbContextOptions<OrdersContext> options, IMediatorHandler mediatorHandler)
        : base(options)
    {
        _mediatorHandler = mediatorHandler;
    }

    public DbSet<Voucher> Vouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        modelBuilder.Ignore<Event>();
        modelBuilder.Ignore<ValidationResult>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersContext).Assembly);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.HasSequence<int>("MySequence").StartsAt(1000).IncrementsBy(1);

        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> CommitAsync()
    {
        foreach (var entry in ChangeTracker.Entries()
            .Where(entry => entry.Entity.GetType().GetProperty("CreatedOn") != null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedOn").CurrentValue = DateTime.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("CreatedOn").IsModified = false;
            }
        }

        var success = await base.SaveChangesAsync() > 0;
        return success;
    }
}
