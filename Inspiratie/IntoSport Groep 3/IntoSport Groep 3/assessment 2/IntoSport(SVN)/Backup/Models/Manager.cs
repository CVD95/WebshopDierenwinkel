using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntoSport.Models
{
    public class Manager
    {
        public String Productnaam { get; set; }
        public double Omzet { get; set; }
        public double Winst { get; set; }
        public int Maand { get; set; }
        public int Jaar { get; set; }

        public String maandconverter(int maand)
        {
            String Maand = "";

            switch (maand)
            {
                case 1: Maand = "Januari"; break;
                case 2: Maand = "Februari"; break;
                case 3: Maand = "Maart"; break;
                case 4: Maand = "April"; break;
                case 5: Maand = "Mei"; break;
                case 6: Maand = "Juni"; break;
                case 7: Maand = "Juli"; break;
                case 8: Maand = "Augustus"; break;
                case 9: Maand = "September"; break;
                case 10: Maand = "October"; break;
                case 11: Maand = "Novmeber"; break;
                case 12: Maand = "December"; break;
                default: Maand = ""; break;
            }
            return Maand;
        }

        public String toString()
        {

            return Productnaam + "" + Omzet.ToString() + "" + Winst.ToString();

        }

    }
}