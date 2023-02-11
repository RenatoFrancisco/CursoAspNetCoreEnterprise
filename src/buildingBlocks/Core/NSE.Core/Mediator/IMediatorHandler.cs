namespace NSE.Core.Mediator;

public interface IMediatorHandler
{
    Task PublishEventAsync<T>(T @event) where T : Event;

    Task<ValidationResult> SendCommand<T>(T command) where T : Command;
}
