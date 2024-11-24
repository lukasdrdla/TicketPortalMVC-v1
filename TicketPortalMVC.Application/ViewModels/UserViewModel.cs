using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels
{
    public class UserViewModel
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

        // Role uživatele
        [Display(Name = "Role")]
        public string Role { get; set; }

        // Pro zobrazení detailů
        public string Id { get; set; } // Id uživatele

        // Možnost zobrazení roli pro editaci
        public List<string> AllRoles { get; set; } = new List<string>(); // seznam všech rolí

    }
}
