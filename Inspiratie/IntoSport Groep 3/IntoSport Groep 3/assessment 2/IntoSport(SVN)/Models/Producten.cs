using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IntoSport.Models
{
    public class Producten
    {
        public int productCode { get; set; }

        [Required(ErrorMessage = "Naam is een verplicht veld")]
        [StringLength(45, ErrorMessage = "De naam mag niet langer zijn dan 45 karakters")]
        public String productNaam { get; set; }

        [Required(ErrorMessage = "merk is een verplicht veld")]
        [StringLength(45, ErrorMessage = "De merknaam mag niet langer zijn dan 45 karakters")]
        public String productMerk { get; set; }

        //[Required(ErrorMessage = "maat is een verplicht veld")]
        //[StringLength(45, ErrorMessage = "De maat mag niet langer zijn dan 10 karakters")]
        //public String productMaat { get; set; }

        [Required(ErrorMessage = "Productype is een verplicht veld")]
        [StringLength(20, ErrorMessage = "Producttype mag maximaal 20 Karakters lang zijn")]
        public String productType { get; set; }

        [Required(ErrorMessage = "Inkoopprijs is een verplicht veld")]
        [Range(0, double.MaxValue, ErrorMessage = "De inkoopprijs moet een positief getal zijn")]
        public double productInkoopprijs { get; set; }

        [Required(ErrorMessage = "Verkoopprijs is een verplicht veld")]
        [Range(0, double.MaxValue, ErrorMessage = "De verkoopprijs moet een positief getal zijn")]
        public double productVerkoopprijs { get; set; }

        [Required(ErrorMessage = "Voorraad is een verplicht veld")]
        [Range(0, int.MaxValue, ErrorMessage = "Vooraad mag niet kleiner dan 0 zijn")]
        public int productVoorraad { get; set; }

        [Required(ErrorMessage = "Aanbiedingscode is een verplicht veld")]
        [Range(0, int.MaxValue, ErrorMessage = "Aanbiedingscode moet bestaan in de database")]
        public int productAanbiedingscode { get; set; }

        [Required(ErrorMessage = "Omschrijving is een verplicht veld")]
        [StringLength(50, ErrorMessage = "Omschrijving mag maximaal 50 Karakters lang zijn")]
        public String productOmschrijving { get; set; }


        public String productAanbiedingsnaam;

        public SelectList Maten { get; set; }

        //[Range(1, Double.MaxValue, ErrorMessage = "Het is verplicht om een maat te selecteren!")]
        public int selectedMaatID { get; set; }
    }
}