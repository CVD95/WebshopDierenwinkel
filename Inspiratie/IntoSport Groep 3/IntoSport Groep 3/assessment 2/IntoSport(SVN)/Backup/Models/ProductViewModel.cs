using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IntoSport.Models
{
    public class ProductViewModel
    {
        public List<Producten> productenList { get; set; }

        public Producten producten { get; set; }
        public Aanbieding aanbiedingen { get; set; }
        public MaatModel maten { get; set; }

        public SelectList aanbieding { get; set; }
        public SelectList maat { get; set; }

        //[Range(1, Double.MaxValue, ErrorMessage = "Het is verplicht om een maat te selecteren!")]
        public int selectedMaatID { get; set; }

        [Range(1, Double.MaxValue, ErrorMessage = "Het is verplicht om een aanbieding te selecteren")]
        public int SelectedaanbiedingID { get; set; }

    }
}