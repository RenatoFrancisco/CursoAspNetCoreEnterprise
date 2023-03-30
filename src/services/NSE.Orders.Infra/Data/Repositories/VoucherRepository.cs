namespace NSE.Orders.Infra.Data.Repositories;

public class VoucherRepository : IVoucherRepository
{
    private readonly OrdersContext _context;

    public VoucherRepository(OrdersContext context) => _context = context;

    public IUnitOfWork UnitOfWork => _context;

    public async Task<Voucher> GetVoucherByCode(string code) => 
        await _context.Vouchers.FirstOrDefaultAsync(p => p.Code == code);

    public void Update(Voucher voucher) => _context.Vouchers.Update(voucher);

    public void Dispose() => _context.Dispose();
}
