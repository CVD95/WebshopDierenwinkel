using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IntoSport.Models
{
    public class RegistrerenModel
    {
        [Required(ErrorMessage = "Een Gebruikersnaam is verplicht")] // Dit MOET aanwezig zijn
        [StringLength(25, ErrorMessage = "Een Gebruikersnaam mag maximaal 25 karakters hebben")]
        public string Gebruikersnaam { get; set; }

        [Required(ErrorMessage = "Een Wachtwoord is verplicht")]
        [StringLength(25, ErrorMessage = "Een Wachtwoord mag maximaal 25 karakters hebben")]
        [DataType(DataType.Password)]
        public string Wachtwoord { get; set; }

        [Required(ErrorMessage = "Een Naam is verplicht")]
        [StringLength(25, ErrorMessage = "Een Naam mag maximaal 25 karakters hebben")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Gebruik alleen letters!")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Een Adres is verplicht")]
        [StringLength(50, ErrorMessage = "Een woonplaats mag maximaal 50 karakters hebben")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Gebruik alleen letters!")]
        public string Adres { get; set; }

        [Required(ErrorMessage = "Een Woonplaats is verplicht")]
        [StringLength(50, ErrorMessage = "Een woonplaats mag maximaal 50 karakters hebben")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Gebruik alleen letters!")]
        public string Woonplaats { get; set; }

        [Required(ErrorMessage = "Een Telefoonnummer is verplicht")]
        //[RegularExpression(@"^[0­9]+$", ErrorMessage = "Telefoonnummer mag alleen nummers bevatten")]
        public int Telefoonnummer { get; set; }

        [Required(ErrorMessage = "Een Email is verplicht")]
        [RegularExpression(@"^([\w­\.]+)@((\[[0­9]{1,3}\.[0­9]{1,3}\.[0­9]{1,3}\.)|(([\w­]+\.)+))([a-zA­Z]{2,4}|[0­9]{1,3})(\]?)$", ErrorMessage = "Email moet bestaan!")]
        public string Email { get; set; }
    }
}