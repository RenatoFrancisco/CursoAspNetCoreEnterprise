namespace NSE.Carrinho.API.Model;

public class ItemCart
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }
    public decimal Value { get; set; }
    public string Image { get; set; }
    public Guid CartId { get; set; }

    public ItemCart() => Id = Guid.NewGuid();

    [JsonIgnore]
    public CustomerCart CustomerCart { get; set; }

    internal void AssociateCart(Guid carrinhoId) => CartId = carrinhoId;

    internal decimal CalculateValue() => Amount * Value;

    internal void AddUnits(int unidades) => Amount += unidades;

    internal void UpdateUnits(int unidades) => Amount = unidades;

    internal bool IsValid() => new ItemCartValidation().Validate(this).IsValid;

    public class ItemCartValidation : AbstractValidator<ItemCart>
    {
        public ItemCartValidation()
        {
            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid product id");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("The product name is required");

            RuleFor(c => c.Amount)
                .GreaterThan(0)
                .WithMessage(item => $"The minimum amount for {item.Name} is 1");

            RuleFor(c => c.Amount)
                .LessThanOrEqualTo(CustomerCart.MAX_AMOUNT_ITEM)
                .WithMessage(item => $"The minimum amount for {item.Name} is {CustomerCart.MAX_AMOUNT_ITEM}");

            RuleFor(c => c.Value)
                .GreaterThan(0)
                .WithMessage(item => $"The value of {item.Name} must be grater than 0");
        }
    }
}
