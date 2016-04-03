using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webshop.Models;

namespace Webshop.Functions
{
    public abstract class TotalCostCalculator
    {
        abstract public List<OrderLine> OrderLines { get; set; }
        public decimal TotalCost()
        {
            decimal total = 0;
            if (OrderLines != null)
            {
                foreach (OrderLine ol in OrderLines)
                {
                    total += (ol.Product.Price * ol.Amount);
                }
            }
            return total;
        }
    }
}