using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IntoSport.Models
{
    public class Aanbieding
    {
        public int aanbiedingscode { get; set; }

        [Required(ErrorMessage = "Naam is een verplicht veld")]
        public String naam { get; set; }

        [Required(ErrorMessage = "Korting is een veplicht veld")]
        public int kortingspercentage { get; set; }

        [Required(ErrorMessage = "Datum is een veplicht veld")]
        public DateTime geldig_tot { get; set; }
    }
}