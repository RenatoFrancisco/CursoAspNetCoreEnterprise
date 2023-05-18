using Transaction = NSE.Pagamento.API.Models.Transaction;

namespace NSE.Pagamento.API.Data.Repositories;

public class PaymentRepositiry : IPaymentRepository
{
    private readonly PaymentsContext _context;

    public PaymentRepositiry(PaymentsContext context) => _context = context;

    public IUnitOfWork UnitOfWork => _context;

    public void AddPayment(Payment payment) => _context.Payments.Add(payment);

    public void AddTransaction(Transaction transaction) => _context.Transactions.Add(transaction);

    public async Task<Payment> GetPaymentByOrderIdAsync(Guid orderId)
    {
        return await _context.Payments.AsNoTracking()
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByOrderIdAsync(Guid orderId)
    {
        return await _context.Transactions.AsNoTracking()
            .Where(t => t.Payment.OrderId == orderId).ToListAsync();
    }

    public void Dispose() => _context.Dispose();
}
