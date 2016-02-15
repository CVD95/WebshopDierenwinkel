using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntoSport.Models
{
    public class LayoutModel
    {

        public List<Producten> producten { get; set; }
        public List<Sport> sporten { get; set; }
        public Producten product { get; set; }
        public Sport sport { get; set; }

        public MaatModel maten { get; set; }

        public SelectList maat { get; set; }

        //[Range(1, Double.MaxValue, ErrorMessage = "Het is verplicht om een maat te selecteren!")]
        public int selectedMaatID { get; set; }
    }
}