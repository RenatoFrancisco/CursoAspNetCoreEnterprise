namespace NSE.Pagamento.API.Services;

public interface IPaymentService
{
    Task<ResponseMessage> AuthorizePaymentAsync(Payment payment);
    Task<ResponseMessage> CapturePaymentAsync(Guid orderId);
    Task<ResponseMessage> CancelPayment(Guid orderId);
}
