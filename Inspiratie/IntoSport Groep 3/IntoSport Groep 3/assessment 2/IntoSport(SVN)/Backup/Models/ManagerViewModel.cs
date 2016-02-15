using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntoSport.Models
{
    public class ManagerViewModel
    {

        public List<Manager> overzicht1 { get; set; }
        public List<Manager> topwinst { get; set; }
        public List<Manager> topverlies { get; set; }
        public List<Manager> overzichtPerMaand { get; set; }
    }
}