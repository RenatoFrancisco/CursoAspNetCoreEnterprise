namespace NSE.WebApp.MVC.Models;

public class RegisterUser
{
    [Required(ErrorMessage = "The field {0} is required")]
    [EmailAddress(ErrorMessage = "The field {0} has an invalid format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The field {0} is required")]
    public string Password { get; set; }

    [DisplayName("Confirm your password")]
    [Compare("Password", ErrorMessage = "The passwords does not match")]
    public string ConfirmPassword { get; set; }
}