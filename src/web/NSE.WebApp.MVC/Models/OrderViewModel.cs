namespace NSE.WebApp.MVC.Models;

public class OrderViewModel
{
    #region Order

    public int Code { get; set; }

    // Authorized = 1,
    // Paid = 2,
    // Recused = 3,
    // Delivered = 4,
    // Cancelled = 5
    public int Status { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalValue { get; set; }

    public decimal Discount { get; set; }
    public bool UsedVoucher { get; set; }

    public List<ItemOrderViewModel> ItemsOrder { get; set; } = new List<ItemOrderViewModel>();

    #endregion

    #region Item Order

    public class ItemOrderViewModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
    }

    #endregion

    #region Address

    public AddressViewModel Address { get; set; }

    #endregion
}
