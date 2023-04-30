namespace NSE.Bff.Compras.Models;

public class OrderDTO
{
    #region Order

    public int Code { get; set; }
    // Authorized = 1,
    // Paid = 2,
    // Recused = 3,
    // Delivered = 4,
    // Cacelled = 5
    public int Status { get; set; }
    public DateTime Date { get; set; }

    public decimal TotalValue { get; set; }
    public decimal Discount { get; set; }
    public string VoucherCode { get; set; }
    public bool UsedVoucher { get; set; }

    public List<ItemCartDTO> Items { get; set; } = new List<ItemCartDTO>();

    #endregion

    #region Addres

    public AddressDTO Address { get; set; }

    #endregion

    #region Cartão

    [Required(ErrorMessage = "The card number is required")]
    [DisplayName("Card Number")]
    public string CardNumber { get; set; }

    [Required(ErrorMessage = "The owner name is required")]
    [DisplayName("Owner Name")]
    public string CardName { get; set; }

    [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "The expiration date must be in the format MM/AA")]
    [CardExpiration(ErrorMessage = "Card expired")]
    [Required(ErrorMessage = "The expiration date is required")]
    [DisplayName("Expiration Date MM/AA")]
    public string CardExpiration { get; set; }

    [Required(ErrorMessage = "The sucurity code is required")]
    [DisplayName("Secutiry Code")]
    public string CardCvv { get; set; }

    #endregion
}
