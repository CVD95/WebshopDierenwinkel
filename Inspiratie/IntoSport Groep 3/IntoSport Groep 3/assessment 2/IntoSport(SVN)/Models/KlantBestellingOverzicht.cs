using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IntoSport.Models
{
    public class KlantBestellingOverzicht
    {
        public int FactuurCode { get; set; }
        public String Naam { get; set; }
        public float Totaal { get; set; }

        [Required(ErrorMessage = "Wachtwoord is vereist")]
        public String Status { get; set; }
    }
}