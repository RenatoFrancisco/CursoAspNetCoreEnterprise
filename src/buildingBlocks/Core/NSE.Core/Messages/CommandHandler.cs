using NSE.Core.Data;

namespace NSE.Core.Messages;

public abstract class CommandHandler
{
    protected ValidationResult ValidationResult;

	protected CommandHandler() => ValidationResult = new ValidationResult();

    protected void AddError(string message) =>
        ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));

    protected async Task<ValidationResult> PersistDataAsync(IUnitOfWork uow)
    {
        if (!await uow.CommitAsync())
            AddError("Occured an error while persisting data");

        return ValidationResult;
    }
}
