using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel.DataAnnotations;


using WorkshopASPNETMVC_II_Start.Validation;

namespace WorkshopASPNETMVC_III_Start.Models
{
    public enum Lokatie
    {
        Bredewater, Innovatiefabriek, DenHaag
    }
    
    public class Evenement
    {
        
        private List<Inschrijving> inschrijvingen = new List<Inschrijving>();
        
        public int ID { get; set; }

        [Range(0, 120, ErrorMessage="Leeftijd moet tussen de 0 en 120 liggen")]
        public int Leeftijd { get; set; }
        
        [Required]
        [StringLength(20, ErrorMessage = "De naam mag maximaal 20 karakers bevatten.")]
        public String Naam { get; set; }

        [Required(ErrorMessage = "Datum is een verplicht veld")]
        [FutureDate(ErrorMessage="De ingevoerde datum moet in de toekomst liggen.")]
        public DateTime Datum { get; set; }
        
        [Required(ErrorMessage = "Lokatie is een verplicht veld")]
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
