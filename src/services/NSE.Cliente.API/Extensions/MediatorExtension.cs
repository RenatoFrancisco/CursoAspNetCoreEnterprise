namespace NSE.Cliente.API.Extensions;

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
