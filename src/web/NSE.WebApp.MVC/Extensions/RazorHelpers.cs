namespace NSE.WebApp.MVC.Extensions;

public static class RazorHelpers
{
    public static string HashEmailForGravatar(this RazorPage page, string email)
    {
        var md5Hasher = MD5.Create();
        var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(email));
        var sBuilder = new StringBuilder();

        foreach (var t in data)
            sBuilder.Append(t.ToString("x2"));

        return sBuilder.ToString();
    }

    public static string FormatCurrency(this RazorPage page, decimal valor) => FormatCurrency(valor);

    private static string FormatCurrency(decimal valor) => 
        string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", valor);

    public static string StockMessage(this RazorPage page, int amount) =>
        amount > 0 ? $"Only {amount} in stock!" : "Sold out!";

    public static string UnitsPerProducts(this RazorPage page, int units) => 
        units > 1 ? $"{units} units" : $"{units} unit";

    public static string SelectOptionsByAmount(this RazorPage page, int amount, int selectedValue = 0)
    {
        var sb = new StringBuilder();
        for (var i = 1; i <= amount; i++)
        {
            var selected = "";
            if (i == selectedValue) selected = "selected";
            sb.Append($"<option {selected} value='{i}'>{i}</option>");
        }

        return sb.ToString();
    }
}
