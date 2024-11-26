using System.ComponentModel.DataAnnotations;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels
{
    public class TicketViewModel
    {
        public int TicketId { get; set; }

        [Required(ErrorMessage = "Událost je povinná.")]
        [Display(Name = "Událost")]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Cena je povinná.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cena musí být kladné číslo.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Typ lístku je povinný.")]
        [StringLength(50, ErrorMessage = "Typ lístku může obsahovat maximálně 50 znaků.")]
        [Display(Name = "Typ lístku")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Datum vytvoření je povinné.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Datum vytvoření")]
        public DateTime CreatedAt { get; set; } = DateTime.Now.Date;
        
        public string EventName { get; set; } = string.Empty;
        

        // Seznam všech událostí pro rozbalovací menu
        [Display(Name = "Vyberte událost")]
        public List<Event> Events { get; set; } = new();
    }
}
