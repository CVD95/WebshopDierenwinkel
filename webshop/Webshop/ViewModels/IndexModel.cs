using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webshop.Models;

namespace Webshop.ViewModels
{
    public class IndexModel
    {
        public List<Category> Categories { get; set; }

        public List<Product> Products { get; set; }

        public ProductPageModel ProductPageModel { get; set; }

        public string FrontImage { get; set; }
    }
}