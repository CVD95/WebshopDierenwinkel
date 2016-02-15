using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToetsVraag1.Models
{
    class MobieleTelefoon
    {
        public int ID { get; set; }

        public string Fabrikant { get; set; }

        public string Type { get; set; }

        public double Prijs { get; set; }

        public OperatingSystem OperatingSystem { get; set; }
    }


            public override string ToString()
        {
            return String.Format("Fabrikant = " + Fabrikant + "Type = " + Type +  );
        }

}
