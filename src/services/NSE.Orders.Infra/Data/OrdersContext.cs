namespace NSE.Orders.Infra.Data;

public class OrdersContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediator;

    public OrdersContext(DbContextOptions<OrdersContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        ChangeTracker.AutoDetectChangesEnabled = true;
    }

    public DbSet<Voucher> Vouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Event>();
        modelBuilder.Ignore<ValidationResult>();

        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.HasSequence<int>("MySequence").StartsAt(1000).IncrementsBy(1);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersContext).Assembly);
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

public static class MediatorExtension
{
    public static async Task PublishEventsAsync<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Notifications is not null && x.Entity.Notifications.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Notifications)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearEvents());

        var tasks = domainEvents
            .Select(async (domainEvent) =>
                await mediator.PublishEventAsync(domainEvent));

        await Task.WhenAll(tasks);
    }
}
