using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkshopASPNETMVC_II_Start.Models
{

    public enum Lokatie
    {
        Bredewater, Innovatiefabriek, DenHaag
    }
    
    public class Evenement
    {
   
        private List<Inschrijving> inschrijvingen = new List<Inschrijving>();
        public int ID { get; set; }
        public int Leeftijd { get; set; }
        public String Naam { get; set; }
        public DateTime Datum { get; set; }
        public Lokatie Lokatie { get; set; }
        
        
        public List<Inschrijving> Inschrijvingen { 
            get
            { 
                return inschrijvingen; 
            }
            set
            {
                inschrijvingen = value;
            }
        
        }

        public void AddStudent(Inschrijving student)
        {
            this.inschrijvingen.Add(student);
        }

        public override string ToString()
        {
            return String.Format("{0} (id={1}) {2} {3} ", Naam, ID, Datum, Lokatie);
        }
    }
}
