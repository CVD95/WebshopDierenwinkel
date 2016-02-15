using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WorkshopASPNETMVC_III_Start.Models
{
    public class Game
    {
        [Required(ErrorMessage= "Naam is een verplicht veld")]
        [StringLength(20,ErrorMessage="De naam mag maximaal 20 karakers bevatten.")]
        public String Naam { get; set; }
       
        public Genre Genre { get; set; }

        public int ID { get; set; }

        public override string ToString()
        {
            return String.Format("{0} (id = {1}) {2}", Naam, ID, Genre);
        }
    }
}
