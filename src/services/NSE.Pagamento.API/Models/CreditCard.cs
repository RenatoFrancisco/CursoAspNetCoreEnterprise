namespace NSE.Pagamento.API.Models;

public class CreditCard
{
    public string CardName { get; set; }
    public string CardNumber { get; set; }
    public string ExpirationMonthYear { get; set; }
    public string CVV { get; set; }

    protected CreditCard() { }

    public CreditCard(string cardName, string cardNumber, string expirationMonthYear, string cvv)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        ExpirationMonthYear = expirationMonthYear;
        CVV = cvv;
    }
}
