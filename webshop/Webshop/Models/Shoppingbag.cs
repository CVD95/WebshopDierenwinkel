using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webshop.Functions;

namespace Webshop.Models
{
    public class ShoppingBag : TotalCostCalculator
    {
        public override List<OrderLine> OrderLines { get; set; }
    }
}