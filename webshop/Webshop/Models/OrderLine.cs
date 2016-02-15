using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class OrderLine
    {
        public Product Product { get; set; }
        public ulong Amount { get; set; }
    }
}