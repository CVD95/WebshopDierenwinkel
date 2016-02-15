using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntoSport.Models
{
    public class WinkelwagenViewModel
    {
        public List<WinkelwagenItem> winkelwagenItem { get; set; }
        public double totaal { get; set; }
    }
}