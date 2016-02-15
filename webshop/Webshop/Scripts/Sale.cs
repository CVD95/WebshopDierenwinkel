using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class Sale
    {

        [Required(ErrorMessage = "Een korting moet een afloopdatum hebben")]
        public DateTime dateUntillExpire { get; set; }

        [Required(ErrorMessage = "Aanbiedingsbeschrijving is verplicht.")]
        public string saleText { get; set; }


        [Required(ErrorMessage = "Kortingspercentage is een verplicht veld")]
        [Range(0, 100, ErrorMessage = "Een kortingspercentage moet tussen de 0 en 100 % liggen.")]
        public int salePercentage { get; set;}
    }
}