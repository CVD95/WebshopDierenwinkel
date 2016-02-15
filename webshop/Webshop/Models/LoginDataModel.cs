using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class LoginDataModel
    {
        //Login model wordt gebruikt om alleen het Wachtwoord en de username van een user op te halen

        [Required(ErrorMessage = "Wachtwoord en / of gebruikersnaam is incorrect.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gebruikersnaam is een verplicht veld")]
        public string Username { get; set; }
    }
}