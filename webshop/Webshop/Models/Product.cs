using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class Product
    {
        public ulong Id { get; set; }

        [Required(ErrorMessage = "Naam is een verplicht veld")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Prijs is een verplicht veld")]
        [Range(0, double.MaxValue, ErrorMessage = "De Prijs moet een positief getal zijn")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "inkoopprijs is een verplicht veld")]
        [Range(0, double.MaxValue, ErrorMessage = "De inkoopprijs moet een positief getal zijn")]
        public decimal BuyPrice { get; set; }

        [Required(ErrorMessage = "Voorraad is een verplicht veld")]
        [Range(0, double.MaxValue, ErrorMessage = "De Voorraad moet een positief getal zijn")]
        public ulong Stock { get; set; }

        public Category Category { get; set; }

        [Required(ErrorMessage = "beschrijving va een product is verplicht")]
        [StringLength(300, ErrorMessage = "Productbeschrijving mag niet langer zijn dan 300 karakters")]
        public string ProductDescription { get; set; }
    }
}