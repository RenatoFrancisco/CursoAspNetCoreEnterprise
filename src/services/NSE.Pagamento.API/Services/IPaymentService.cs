namespace NSE.Pagamento.API.Services;

public interface IPaymentService
{
    Task<ResponseMessage> AuthorizePaymentAsync(Payment payment);
    Task<ResponseMessage> CapturePaymentAsync(Guid orderId);
    Task<ResponseMessage> CancellPayment(Guid orderId);
}
