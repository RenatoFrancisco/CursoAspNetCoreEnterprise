namespace NSE.WebApp.MVC.Extensions;

public class CpfAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext) =>
        Cpf.Validate(value.ToString()) ? ValidationResult.Success : new ValidationResult("Cpf with invalid format");
}

public class CpfAttributeAdapter : AttributeAdapterBase<CpfAttribute>
{

    public CpfAttributeAdapter(CpfAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer) { }

    public override void AddValidation(ClientModelValidationContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-cpf", GetErrorMessage(context));
    }

    public override string GetErrorMessage(ModelValidationContextBase validationContext) => "CPF with invalid format";
}

public class CpfValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
{
    private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

    public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
    {
        if (attribute is CpfAttribute CpfAttribute)
            return new CpfAttributeAdapter(CpfAttribute, stringLocalizer);

        return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
    }
}
