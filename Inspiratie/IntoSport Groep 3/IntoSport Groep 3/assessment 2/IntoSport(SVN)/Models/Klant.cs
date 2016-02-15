using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IntoSport.Models
{
    public class Klant
    {
        [Required(ErrorMessage = "Invalide Id")]
        public int Id { get; set; }

        public string Gebruikersnaam { get; set; }

        public string Wachtwoord { get; set; }

        [Required(ErrorMessage = "Een Naam is verplicht")]
        [StringLength(25)]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Een Adres is verplicht")]
        [StringLength(50)]
        public string Adres { get; set; }

        [Required(ErrorMessage = "Een Woonplaats is verplicht")]
        [StringLength(50)]
        public string Woonplaats { get; set; }

        [Required(ErrorMessage = "Een Telefoonnummer is verplicht")]
        //[RegularExpression(@"^[0­9]+$", ErrorMessage = "Telefoonnummer mag alleen nummers bevatten")]
        public string Telefoonnummer { get; set; }

        [Required(ErrorMessage = "Een Email is verplicht")]
        [RegularExpression(@"^([\w­\.]+)@((\[[0­9]{1,3}\.[0­9]{1,3}\.[0­9]{1,3}\.)|(([\w­]+\.)+))([a-zA­Z]{2,4}|[0­9]{1,3})(\]?)$", ErrorMessage = "Email moet bestaan!")]
        public string Email { get; set; }

        public string Rechten { get; set; }
        public DateTime Datum_inschrijving { get; set; } 
    }
}