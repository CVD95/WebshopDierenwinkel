using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class Category
    {
        public ulong Id { get; set; }

        [Required(ErrorMessage = "Categorie Beschrijving is een verplicht veld")]
        [StringLength(45, ErrorMessage = "De categorienaam mag niet langer zijn dan 45 karakters")]
        public string Name { get; set; }
    }
}