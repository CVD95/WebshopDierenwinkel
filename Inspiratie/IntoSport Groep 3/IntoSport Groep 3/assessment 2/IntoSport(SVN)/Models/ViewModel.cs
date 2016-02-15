using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntoSport.Models
{
    public class ViewModel
    {

        public IEnumerable<Manager> overzicht1 { get; set; }
        public IEnumerable<Manager> topwinst { get; set; }
        public IEnumerable<Manager> topverlies { get; set; }
    }
}