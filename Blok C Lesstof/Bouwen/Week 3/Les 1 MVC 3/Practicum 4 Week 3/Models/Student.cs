using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkshopASPNETMVC_II_Start.Models
{
    public class Student
    {
     
        private List<Inschrijving> inschrijvingen = new List<Inschrijving>();

        public int ID { get; set; }
        public string Naam { get; set; }
        public DateTime GeboorteDatum { get; set; }
        public int StudiePunten { get; set; }
        public Game FavorieteSpel { get; set; }
        public List<Inschrijving> Inschrijvingen 
        { 
            get { return inschrijvingen;}
            set { inschrijvingen = value; } 
        }

        public void AddInschrijving(Inschrijving inschrijving)
        {
            inschrijvingen.Add(inschrijving);
        }

        public void AddStudiePunten(int studiePunten)
        {
            StudiePunten += studiePunten;
        }

        public override String ToString()
        {
            return String.Format("{0} (id={1}) geboorteDatum = {2} studiePunten={3} \n favoriete spel = {4}", Naam, ID, GeboorteDatum, StudiePunten, FavorieteSpel);
        }

    }
}
