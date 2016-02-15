using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WorkshopASPNETMVC_II_Start.Models
{
    public class Genre
    {
       
        public string Naam { get; set; }
        public bool Verslavend { get; set; }
        public int ID { get; set; }

        public override String ToString()
        {
            return String.Format("{0} ({1})", Naam,  
                Verslavend ? "Verslavend" : "Niet verslavend");
        }
    }
}

