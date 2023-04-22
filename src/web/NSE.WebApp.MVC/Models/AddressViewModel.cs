namespace NSE.WebApp.MVC.Models;

public class AddressViewModel
{
    [Required]
    public string Street { get; set; }
    [Required]
    public string Number { get; set; }
    public string Complement { get; set; }
    [Required]
    public string Neighborhood { get; set; }
    [Required]
    [DisplayName("Zip Code")]
    public string ZipCode { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string State { get; set; }

    public override string ToString() => $"{Street}, {Number} {Complement} - {Neighborhood} - {City} - {State}";
}
