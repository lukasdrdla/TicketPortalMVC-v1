using System.ComponentModel.DataAnnotations;

namespace TicketPortalMVC.Application.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email je povinný")]
    [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Heslo je povinné")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    
}