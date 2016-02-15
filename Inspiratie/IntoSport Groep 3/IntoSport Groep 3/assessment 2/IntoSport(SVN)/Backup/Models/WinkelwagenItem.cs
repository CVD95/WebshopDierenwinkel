using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntoSport.Models
{
    public class WinkelwagenItem
    {
        public WinkelwagenItem(Producten product, int kwantiteit)
        {
            this.product = product;
            this.kwantiteit = kwantiteit;
        }
        public WinkelwagenItem(Producten product, int kwantiteit, string maat, string kleur)
        {
            this.product = product;
            this.kwantiteit = kwantiteit;
            this.kleur = kleur;
            this.maat = maat;
        }
        public Producten product { get; set; }
        public int kwantiteit { get; set; }
        public string kleur { get; set; }
        public string maat { get; set; }
    }
}