using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WorkshopASPNETMVC_II_Start.Models
{
    public class Game
    {
      
        public String Naam { get; set; }
       
        public Genre Genre { get; set; }

        public int ID { get; set; }

        public override string ToString()
        {
            return String.Format("{0} (id = {1}) {2}", Naam, ID, Genre);
        }
    }
}
