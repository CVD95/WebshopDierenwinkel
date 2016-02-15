using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkshopASPNETMVC_II_Start.Models
{
    public class Inschrijving
    {
        public Student Student { get; set; }
        public Evenement Evenement { get; set; }
        public bool Betaald { get; set; }
        public bool EetMee { get; set; }

        public override string ToString()
        {
            return String.Format("{0} heeft zich ingeschreven voor {1} \n De student {2} en {3}" , Student, Evenement, Betaald? "heeft betaald" : "heeft niet betaald", EetMee? "eet mee" : "eet niet mee");
        }

    }
}
