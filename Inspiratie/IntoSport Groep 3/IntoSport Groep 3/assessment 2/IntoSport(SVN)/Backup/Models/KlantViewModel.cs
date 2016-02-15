using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntoSport.Models
{
    public class KlantViewModel
    {
        public List<KlantBestellingOverzicht> bestellingOverzicht { get; set; }
        public List<Klant> klantOverzicht { get; set; }
    }   
}