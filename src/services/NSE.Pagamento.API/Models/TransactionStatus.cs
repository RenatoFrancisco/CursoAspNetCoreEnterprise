namespace NSE.Pagamento.API.Models;

public enum TransactionStatus
{
    Authorozed = 1,
    Paid,
    Recused,
    Reversed,
    Cancelled
}
