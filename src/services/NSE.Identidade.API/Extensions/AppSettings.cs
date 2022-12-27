namespace NSE.Identidade.API.Extensions;

public class AppSettings
{
    public string Secret { get; set; }
    public int ExpirationInHours { get; set; }
    public string Issuer { get; set; }
    public string ValidAt { get; set; }
}