using Transaction = NSE.Pagamentos.NerdsPag.Transaction;

namespace NSE.Pagamento.API.Facade;

public class CreditCardPaymentFacade : IPaymentFacade
{
    private readonly PaymentConfig _paymentConfig;

    public CreditCardPaymentFacade(IOptions<PaymentConfig> paymentConfig) => _paymentConfig = paymentConfig.Value;

    public async Task<Transaction> AuthorizePaymentAsync(Payment payment)
    {
        var nerdsPagSvc = new NerdsPagService(_paymentConfig.DefaultApiKey, _paymentConfig.DefaultEncryptionKey);

        var cardHashGen = new CardHash(nerdsPagSvc)
        {
            CardNumber = payment.CreditCard.CardNumber,
            CardHolderName = payment.CreditCard.CardName,
            CardExpirationDate = payment.CreditCard.ExpirationMonthYear,
            CardCvv = payment.CreditCard.CVV
        };

        var cardHash = cardHashGen.Generate();

        var transaction = new Transaction(nerdsPagSvc)
        {
            CardHash = cardHash,
            CardNumber = payment.CreditCard.CardNumber,
            CardHolderName = payment.CreditCard.CardName,
            CardExpirationDate = payment.CreditCard.ExpirationMonthYear,
            CardCvv = payment.CreditCard.CVV,
            PaymentMethod = PaymentMethod.CreditCard,
            Amount = payment.Value
        };

        return ToTransaction(await transaction.AuthorizeCardTransaction());
    }

    public async Task<Transaction> CapturePaymentAsync(Transaction transaction)
    {
        var nerdsPagSvc = new NerdsPagService(_paymentConfig.DefaultApiKey, _paymentConfig.DefaultEncryptionKey);

        var transac = ToTransaction(transaction, nerdsPagSvc);

        return ToTransaction(await transac.CaptureCardTransaction());
    }

    public async Task<Transaction> CancellAuthorizationAsync(Transaction transaction)
    {
        var nerdsPagSvc = new NerdsPagService(_paymentConfig.DefaultApiKey, _paymentConfig.DefaultEncryptionKey);

        var transac = ToTransaction(transaction, nerdsPagSvc);

        return ToTransaction(await transac.CancelAuthorization());
    }

    public static Transaction ToTransaction(Transaction transaction)
    {
        return new Transaction
        {
            Id = Guid.NewGuid(),
            Status = transaction.Status,
            Amount = transaction.Amount,
            CardBrand = transaction.CardBrand,
            AuthorizationCode = transaction.AuthorizationCode,
            Cost = transaction.Cost,
            CardExpirationDate = transaction.CardExpirationDate,
            Nsu = transaction.Nsu,
            Tid = transaction.Tid
        };
    }

    public static Transaction ToTransaction(Transaction transaction, NerdsPagService nerdsPagService)
    {
        return new Transaction(nerdsPagService)
        {
            Status = transaction.Status,
            Amount = transaction.Amount,
            CardBrand = transaction.CardBrand,
            AuthorizationCode = transaction.AuthorizationCode,
            Cost = transaction.Cost,
            Nsu = transaction.Nsu,
            Tid = transaction.Tid
        };
    }
}
