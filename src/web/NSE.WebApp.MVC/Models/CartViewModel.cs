namespace NSE.WebApp.MVC.Models;

public class CartViewModel
{
    public decimal TotalValue { get; set; }
    public VoucherViewModel Voucher { get; set; }
    public bool UsedVoucher { get; set; }
    public decimal Discount { get; set; }
    public List<ItemCartViewModel> Items { get; set; } = new List<ItemCartViewModel>();
}
