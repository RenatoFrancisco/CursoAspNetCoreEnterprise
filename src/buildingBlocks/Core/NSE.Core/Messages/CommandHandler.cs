namespace NSE.Core.Messages;

public abstract class CommandHandler
{
    protected ValidationResult ValidationResult;

	public CommandHandler() => ValidationResult = new ValidationResult();

    public void AddError(string message) =>
        ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
}
