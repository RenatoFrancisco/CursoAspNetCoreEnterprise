namespace NSE.Pagamento.API.Facade;

public interface IPaymentFacade
{
    Task<Transaction> AuthorizePaymentAsync(Payment payment);
    Task<Transaction> CapturePaymentAsync(Transaction transaction);
    Task<Transaction> CancellAuthorizationAsync(Transaction transaction);
}
