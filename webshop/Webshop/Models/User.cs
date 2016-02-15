using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using Webshop.Enums;

namespace Webshop.Models
{
    public class User
    {
        public ulong Id { get; set; }

        [Required(ErrorMessage = "Gebruikersnaam is verplicht en mag niet kleiner zijn dan 3 tekens")]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string Username { get; set; }

        [Required(ErrorMessage = "wachtwoord is verplicht en mag niet kleiner zijn dan 3 tekens")]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is verplicht")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail bevat tekens die niet in een Email adres mogen.")]
        public string Email { get; set; }
        //Datatype Email adres is makkelijker om te gebruiken dan een REGEX

        [Required(ErrorMessage = "Voornaam is een verplicht veld En mag niet kleiner zijn dan 2 tekens")]
        [StringLength(int.MaxValue, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Achternaam is een verplicht veld en mag niet kleiner zijn dan 2 tekens")]
        [StringLength(int.MaxValue, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Adres is verplicht")]
        public Address Address { get; set; }
       
        [Required(ErrorMessage = "geboortedatum is verplicht")]
        public DateTime DateOfBirth { get; set; }

        public string dateTimeString { get; set; }

        public bool TrySetDateTimeString(string date)
        {
            DateTimeFormatInfo info = new DateTimeFormatInfo();
            info.FullDateTimePattern = "M/d/YYYY";

            DateTime newDateTime;
            if (DateTime.TryParse(date, info, DateTimeStyles.AssumeLocal, out newDateTime))
            {
                DateOfBirth = newDateTime;
                return true;
            }
            return false;
        }

        public void Prepare()
        {
            TrySetDateTimeString(dateTimeString);
        }

        public UserRole Role { get; set; }
    }
}