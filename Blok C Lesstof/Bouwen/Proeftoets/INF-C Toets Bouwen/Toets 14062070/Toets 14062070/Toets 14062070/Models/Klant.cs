using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toets.Models
{

    public enum Geslacht
    {
        Man, Vrouw
    }

    public class Klant
    {

        public string Naam { get; set; }
        public string Woonplaats { get; set; }
        public DateTime Geboortedatum { get; set; }
        public string beschrijving { get; set; }
        public Geslacht geslacht { get; set; }

        public override string ToString()
        {
            return String.Format("Naam = " + Naam + " Woonplaats = " +  Woonplaats + " Geboortedatum = " + Geboortedatum +  " Geslacht + " + geslacht + " Bechrijving = " + beschrijving);
        }
    }
}
