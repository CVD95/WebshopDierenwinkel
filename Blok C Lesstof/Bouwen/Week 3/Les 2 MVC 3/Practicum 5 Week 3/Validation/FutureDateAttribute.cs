using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;

namespace WorkshopASPNETMVC_II_Start.Validation
{
    public class FutureDateAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            return value is DateTime && (DateTime)value > DateTime.Now;
        }

    }
}
