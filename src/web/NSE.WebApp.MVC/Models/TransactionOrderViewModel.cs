namespace NSE.WebApp.MVC.Models;

public class TransactionOrderViewModel
{
    #region Order

    public decimal TotalValue { get; set; }
    public decimal Discount { get; set; }
    public string VoucherCode { get; set; }
    public bool UsedVoucher { get; set; }

    public List<ItemCartViewModel> Items { get; set; } = new List<ItemCartViewModel>();

    #endregion

    #region Address

    public AddressViewModel Address { get; set; }

    #endregion

    #region Cart

    [Required(ErrorMessage = "The card number is required")]
    [DisplayName("Card Number")]
    public string CardNumber { get; set; }

    [Required(ErrorMessage = "The owner name is required")]
    [DisplayName("Owner Name")]
    public string CardName { get; set; }

    [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "The expiration date must be in the format MM/AA")]
    //[CartaoExpiracao(ErrorMessage = "Cartão Expirado")]
    [Required(ErrorMessage = "The expiration date is required")]
    [DisplayName("Expiration Date MM/AA")]
    public string CardExpiration { get; set; }

    [Required(ErrorMessage = "The sucurity code is required")]
    [DisplayName("Secutiry Code")]
    public string CardCvv { get; set; }

    #endregion
}
