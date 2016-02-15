using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntoSport.Models
{
    public class ProductSportViewModel
    {
        public List<Sport> sporten { get; set; }
        public List<Producten> producten { get; set; }

        public MaatModel maten { get; set; }

        public SelectList maat { get; set; }

        //[Range(1, Double.MaxValue, ErrorMessage = "Het is verplicht om een maat te selecteren!")]
        public int selectedMaatID { get; set; }
    }
}