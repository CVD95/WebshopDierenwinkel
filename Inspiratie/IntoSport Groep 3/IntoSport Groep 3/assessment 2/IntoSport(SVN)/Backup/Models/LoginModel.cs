using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace IntoSport.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Gebruikersnaam is vereist")] // Dit MOET aanwezig zien
        public string UserName { get; set; }

        [Required(ErrorMessage = "Wachtwoord is vereist")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
    }
}