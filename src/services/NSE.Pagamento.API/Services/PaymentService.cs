namespace NSE.Pagamento.API.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentFacade _paymentFacade;
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentFacade pagamentoFacade,
                          IPaymentRepository pagamentoRepository)
    {
        _paymentFacade = pagamentoFacade;
        _paymentRepository = pagamentoRepository;
    }

    public async Task<ResponseMessage> AuthorizePaymentAsync(Payment payment)
    {
        var transaction = await _paymentFacade.AuthorizePaymentAsync(payment);
        var validationResult = new ValidationResult();

        if (transaction.Status != Models.TransactionStatus.Authorized)
        {
            validationResult.Errors.Add(new ValidationFailure("Payment",
                    "Refused Paymento, contact your card administrator company"));

            return new ResponseMessage(validationResult);
        }

        payment.AddTransaction(transaction);
        _paymentRepository.AddPayment(payment);

        if (!await _paymentRepository.UnitOfWork.CommitAsync())
        {
            validationResult.Errors.Add(new ValidationFailure("Payment",
                "Ocurred an error while processing payment."));

            // Cancell payment in the gateway
            await CancelPayment(payment.OrderId);

            return new ResponseMessage(validationResult);
        }

        return new ResponseMessage(validationResult);
    }

    public async Task<ResponseMessage> CapturePaymentAsync(Guid orderId)
    {
        var transactions = await _paymentRepository.GetTransactionsByOrderIdAsync(orderId);
        var authorizedTransaction = transactions?.FirstOrDefault(t => t.Status == Models.TransactionStatus.Authorized);
        var validationResult = new ValidationResult();

        if (authorizedTransaction is null) throw new DomainException($"Transaction not found for order {orderId}");

        var transaction = await _paymentFacade.CapturePaymentAsync(authorizedTransaction);

        if (transaction.Status != Models.TransactionStatus.Paid)
        {
            validationResult.Errors.Add(new ValidationFailure("Payment",
                $"It was unable to capture the payment of order {orderId}"));

            return new ResponseMessage(validationResult);
        }

        transaction.PaymentId = authorizedTransaction.PaymentId;
        _paymentRepository.AddTransaction(transaction);

        if (!await _paymentRepository.UnitOfWork.CommitAsync())
        {
            validationResult.Errors.Add(new ValidationFailure("Payment",
                $"It was unable to persist the capturing order's payment {orderId}"));

            return new ResponseMessage(validationResult);
        }

        return new ResponseMessage(validationResult);
    }

    public async Task<ResponseMessage> CancelPayment(Guid orderId)
    {
        var transactions = await _paymentRepository.GetTransactionsByOrderIdAsync(orderId);
        var authorizedTransaction = transactions?.FirstOrDefault(t => t.Status == Models.TransactionStatus.Authorized);
        var validationResult = new ValidationResult();

        if (authorizedTransaction == null) throw new DomainException($"Transaction not found for order {orderId}");

        var transaction = await _paymentFacade.CancellAuthorizationAsync(authorizedTransaction);

        if (transaction.Status != Models.TransactionStatus.Cancelled)
        {
            validationResult.Errors.Add(new ValidationFailure("Payment",
                $"It was unable to cancell the payment of order {orderId}"));

            return new ResponseMessage(validationResult);
        }

        transaction.PaymentId = authorizedTransaction.PaymentId;
        _paymentRepository.AddTransaction(transaction);

        if (!await _paymentRepository.UnitOfWork.CommitAsync())
        {
            validationResult.Errors.Add(new ValidationFailure("Payment",
                $"It was unable to persist the cancelling order's payment {orderId}"));

            return new ResponseMessage(validationResult);
        }

        return new ResponseMessage(validationResult);
    }
}
