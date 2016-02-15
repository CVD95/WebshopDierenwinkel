using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IntoSport.Models
{
    public class Sport
    {
        public int sportcode { get; set; }

        [Required(ErrorMessage = "Sportnaam is een verplicht veld.")]
        public String type { get; set; }

        [Required(ErrorMessage = "Type Sport is een verplicht veld.")]
        public String naam { get; set; }
    }
}