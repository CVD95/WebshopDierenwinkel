using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IntoSport.Models
{
    public class BestellingOverzicht
    {
        public String Status { get; set; }
        public double Totaal { get; set; }
        public int Klant_code { get; set; }
        public int Product_code { get; set; }
        public int Factuur_code { get; set; }
        public String guid { get; set; }

        public SelectList ListStatus { get; set; }

        [Range(1, Double.MaxValue, ErrorMessage = "Het is verplicht om een status te selecteren")]
        public string SelectedStatus { get; set; }

        public string klantnaam { get; set; }
    }
}