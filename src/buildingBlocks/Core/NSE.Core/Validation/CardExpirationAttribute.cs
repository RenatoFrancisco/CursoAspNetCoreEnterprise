namespace NSE.Core.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class CardExpirationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
            return false;

        var monthPart = value.ToString().Split('/')[0];
        var yearPart = $"20{value.ToString().Split('/')[1]}";

        if (int.TryParse(monthPart, out var month) &&
            int.TryParse(yearPart, out var year))
        {
            var d = new DateTime(year, month, 1);
            return d > DateTime.UtcNow;
        }

        return false;
    }
}