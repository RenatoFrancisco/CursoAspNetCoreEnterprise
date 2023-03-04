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

    public static string StockMessage(this RazorPage page, int quantidade) =>
        quantidade > 0 ? $"Only {quantidade} in stock!" : "Sold out!";

    public static string UnitsPerProducts(this RazorPage page, int unidades) => 
        unidades > 1 ? $"{unidades} unidades" : $"{unidades} unidade";

    public static string SelectOptionsByAmount(this RazorPage page, int quantidade, int valorSelecionado = 0)
    {
        var sb = new StringBuilder();
        for (var i = 1; i <= quantidade; i++)
        {
            var selected = "";
            if (i == valorSelecionado) selected = "selected";
            sb.Append($"<option {selected} value='{i}'>{i}</option>");
        }

        return sb.ToString();
    }
}
