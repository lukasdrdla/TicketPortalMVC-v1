using System.ComponentModel.DataAnnotations;

namespace TicketPortalMVC.Application.ViewModels;

public class UserCreateViewModel
{
    [Required(ErrorMessage = "Křestní jméno je povinné")]
    [StringLength(50, ErrorMessage = "Křestní jméno může obsahovat maximálně 50 znaků")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Příjmení je povinné")]
    [StringLength(50, ErrorMessage = "Příjmení může obsahovat maximálně 50 znaků")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Adresa je povinná")]
    [StringLength(100, ErrorMessage = "Adresa může obsahovat maximálně 100 znaků")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Město je povinné")]
    [StringLength(50, ErrorMessage = "Město může obsahovat maximálně 50 znaků")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Země je povinná")]
    [StringLength(50, ErrorMessage = "Země může obsahovat maximálně 50 znaků")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "PSČ je povinné")]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "PSČ musí obsahovat 5 číslic")]
    public string PostalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefonní číslo je povinné")]
    [Phone(ErrorMessage = "Neplatné telefonní číslo")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email je povinný")]
    [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Heslo je povinné")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Heslo musí obsahovat alespoň 6 znaků")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Potvrzení hesla je povinné")]
    [Compare("Password", ErrorMessage = "Hesla se neshodují")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
    public string Role { get; set; }
    
    public List<string> Roles { get; set; } = new();
    
}