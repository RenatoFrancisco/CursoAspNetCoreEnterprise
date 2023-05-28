namespace NSE.Pagamento.API.Models;

public enum TransactionStatus
{
    Authorized = 1,
    Paid,
    Recused,
    Reversed,
    Cancelled
}
