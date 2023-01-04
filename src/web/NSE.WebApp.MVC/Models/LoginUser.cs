namespace NSE.WebApp.MVC.Models;

public class LoginUser
{
    [Required(ErrorMessage = "The field {0} is required")]
    [EmailAddress(ErrorMessage = "The field {0} has an invalid format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The field {0} is required")]
    [StringLength(100, ErrorMessage = "The field {0} must contain between {2} and {1} caracteres", MinimumLength = 6)]
    public string Password { get; set; }
}