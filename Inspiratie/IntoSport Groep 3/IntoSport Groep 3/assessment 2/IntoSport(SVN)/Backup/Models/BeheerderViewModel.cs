using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IntoSport.Models
{
    public class BeheerderViewModel
    {
        public List<BestellingOverzicht> bestellingOverzicht { get; set; }
        public List<Klant> klantOverzicht { get; set; }
        public List<Producten> productenOverzicht { get; set; }
        public List<CategorieModel> categorieOverzicht { get; set; }
        public List<Aanbieding> aanbiedingOverzicht { get; set; }
        public List<Sport> sportoverzicht { get; set; }

        public CategorieModel categorie { get; set; }
        public Producten producten { get; set; }
        public Sport sporten { get; set; }

        public SelectList sport { get; set; }
        public SelectList product { get; set; }
        public SelectList aanbieding { get; set; }
        public SelectList maat { get; set; }

        [Range(1, Double.MaxValue, ErrorMessage = "Het is verplicht om een product te selecteren")]
        public int SelectedproductID { get; set; }

        [Range(1, Double.MaxValue, ErrorMessage = "Het is verplicht om een sport te selecteren")]
        public int SelectedSportID { get; set; }
/*
        [Range(1, Double.MaxValue, ErrorMessage = "Het is verplicht om een aanbieding te selecteren")]
        public int SelectedAanbiedingID { get; set; }

        [Range(1, Double.MaxValue, ErrorMessage = "Het is verplicht om een maat te selecteren")]
        public int SelectedMaatID { get; set; }
 */
    }
}