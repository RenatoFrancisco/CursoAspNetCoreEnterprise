using FluentValidation;

namespace NSE.Pedido.API.Application.Commands;

public class AddOrderCommand : Command 
{
    // Order
    public Guid CustomerId { get; set; }
    public decimal TotalValue { get; set; }
    public List<ItemOrder> ItemsOrder { get; set; }

    // Voucher
    public string VoucherCode { get; set; }
    public bool UsedVoucher { get; set; }
    public decimal Discount { get; set; }

    // Address
    public AddressDTO Addres { get; set; }

    // Card
    public string CardNumber { get; set; }
    public string CardName { get; set; }
    public string CardExpitarion { get; set; }
    public string CardCvv { get; set; }

    public override bool IsValid()
    {
        ValidationResult = new AdicionarPedidoValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AdicionarPedidoValidation : AbstractValidator<AddOrderCommand>
    {
        public AdicionarPedidoValidation()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("The Customer Id is invalid.");

            RuleFor(c => c.ItemsOrder.Count)
                .GreaterThan(0)
                .WithMessage("The Order must have at least 1 item.");

            RuleFor(c => c.TotalValue)
                .GreaterThan(0)
                .WithMessage("The Order Value is invald.");

            RuleFor(c => c.CardNumber)
                .CreditCard()
                .WithMessage("The Card Number is invalid.");

            RuleFor(c => c.CardName)
                .NotNull()
                .WithMessage("The Card Owner's Name is required.");

            RuleFor(c => c.CardCvv.Length)
                .GreaterThan(2)
                .LessThan(5)
                .WithMessage("The Card CVV must have 3 or 4 digits.");

            RuleFor(c => c.CardExpitarion)
                .NotNull()
                .WithMessage("The Card Expiration Date is required.");
        }
    }
}
