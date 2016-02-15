using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class ProductPageModel
    {
        public int PageNumber { get; set; }
        public Category Category { get; set; }
    }
}