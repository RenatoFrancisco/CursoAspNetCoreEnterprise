namespace NSE.Carrinho.API.Model;

public class CustomerCart
{
    internal const int MAX_AMOUNT_ITEM = 5;

    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal TotalValue { get; set; }
    public List<ItemCart> Items { get; set; } = new List<ItemCart>();
    public ValidationResult ValidationResult { get; set; }

    public bool UsedVoucher { get; set; }
    public decimal Discount { get; set; }

    public Voucher Voucher { get; set; }

    public CustomerCart(Guid clienteId)
    {
        Id = Guid.NewGuid();
        CustomerId = clienteId;
    }

    public CustomerCart() { }

    public void ApplyVoucher(Voucher voucher)
    {
        Voucher = voucher;
        UsedVoucher = true;
        CalculateCartValue();
    }

    internal void CalculateCartValue()
    {
        TotalValue = Items.Sum(p => p.CalculateValue());
        CalculateTotalDiscountValue();
    }

    private void CalculateTotalDiscountValue()
    {
        if (!UsedVoucher) return;

        decimal discount = 0;
        var value = TotalValue;

        if (Voucher.DiscountType == VoucherDiscountType.Percetage)
        {
            if (Voucher.Percentual.HasValue)
            {
                discount = (value * Voucher.Percentual.Value) / 100;
                value -= discount;
            }
        }
        else
        {
            if (Voucher.DiscountedValue.HasValue)
            {
                discount = Voucher.DiscountedValue.Value;
                value -= discount;
            }
        }

        TotalValue = value < 0 ? 0 : value;
        Discount = discount;
    }

    internal bool ExistentItemCart(ItemCart item) => Items.Any(p => p.ProductId == item.ProductId);

    internal ItemCart GetByProductId(Guid produtoId) =>  Items.FirstOrDefault(p => p.ProductId == produtoId);

    internal void AddItem(ItemCart item)
    {
        item.AssociateCart(Id);

        if (ExistentItemCart(item))
        {
            var itemExistent = GetByProductId(item.ProductId);
            itemExistent.AddUnits(item.Amount);

            item = itemExistent;
            Items.Remove(itemExistent);
        }

        Items.Add(item);
        CalculateCartValue();
    }

    internal void UpdateItem(ItemCart item)
    {
        item.AssociateCart(Id);

        var itemExistente = GetByProductId(item.ProductId);

        Items.Remove(itemExistente);
        Items.Add(item);

        CalculateCartValue();
    }

    internal void UpdateUnits(ItemCart item, int unidades)
    {
        item.UpdateUnits(unidades);
        UpdateItem(item);
    }

    internal void RemoveItem(ItemCart item)
    {
        Items.Remove(GetByProductId(item.ProductId));
        CalculateCartValue();
    }

    internal bool IsValid()
    {
        var erros = Items.SelectMany(i => new ItemCart.ItemCartValidation().Validate(i).Errors).ToList();
        erros.AddRange(new CustomerCartalidation().Validate(this).Errors);
        ValidationResult = new ValidationResult(erros);

        return ValidationResult.IsValid;
    }
    public class CustomerCartalidation : AbstractValidator<CustomerCart>
    {
        public CustomerCartalidation()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Customer unknown");

            RuleFor(c => c.Items.Count)
                .GreaterThan(0)
                .WithMessage("The cart does not have items");

            RuleFor(c => c.TotalValue)
                .GreaterThan(0)
                .WithMessage("The total value of cart must be greater than 0");
        }
    }
}


