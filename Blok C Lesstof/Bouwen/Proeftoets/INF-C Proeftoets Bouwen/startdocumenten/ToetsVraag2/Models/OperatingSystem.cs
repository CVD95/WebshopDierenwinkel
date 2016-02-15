using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToetsVraag1
{
    class OperatingSystem
    {
        public int ID { get; set; }
        public string Naam { get; set; }
        public string Versie { get; set; }
        


        public override string ToString()
        {
            return String.Format("Naam = {0},  Versie = {1}", Naam, Versie);
        }

       
    }
}
