namespace NSE.Pagamento.API.Models;

public interface IPaymentRepository : IRepository<Payment>
{
    void AddPayment(Payment payment);
    void AddTransaction(Transaction transaction);
    Task<Payment> GetPaymentByOrderIdAsync(Guid orderId);
    Task<IEnumerable<Transaction>> GetTransactionsByOrderIdAsync(Guid orderId);
}
