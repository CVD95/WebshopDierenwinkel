using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Webshop.Enums;

namespace Webshop.Models
{
    public class Address
    {
        [Required(ErrorMessage = "Postcode is verplicht")]
        [RegularExpression("^[1-9][0-9]{3} ?(?!sa|sd|ss)[a-zA-Z]{2}$", ErrorMessage = "Verkeerd format")]
        public string PostalCode { get; set; }
        //sa, ss, sd aren't allowed in the Netherlands

        [Required(ErrorMessage = "huisnummer is verplicht")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Huisnummer(s) moet(en) positief zijn")]
        public int HouseNumber { get; set; }
        //Regex any number of 0 to 9 numbers.

        private string suffix;

        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Huisletter moet een letter zijn")]
        public string Suffix
        {
            get

            { //The get keyword defines an accessor method in a property or indexer that retrieves the value of the property or the indexer element. For more information,

                return suffix;
            }
            set
            {
                if(value == null)
                {
                    suffix = "a";
                }
                else
                {
                    suffix = value;
                }
            }
        }

            [Required(ErrorMessage = "Straatnaam is verplicht")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Mag alleen uit letters bestaan.")]
        public string Street { get; set; }
        //REGEX : Alle letters A-Z en a-z

        [Required(ErrorMessage = "Stad / dorp is verplicht")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Mag alleen uit letters bestaan.")]
        //REGEX : Alle letters A-Z en a-z
        public string City { get; set; }

        public AddressType Type { get; set; }
    }
}