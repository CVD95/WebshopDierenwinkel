using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IntoSport.Models
{
    public class CategorieModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Sport is een veplicht veld")]
        public int sport { get; set; }

        [Required(ErrorMessage = "Product is een veplicht veld")]
        public int product { get; set; }

        [Required(ErrorMessage = "Categorietype is een veplicht veld")]
        [StringLength(25, ErrorMessage = "Een type mag maximaal 25 karakters hebben")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Gebruik alleen letters!")]
        public String type { get; set; }

        // ipv ID's, namen laten zien voor overzichten.
        public String productnaam { get; set; }
        public String sportnaam { get; set; }
    }
}