using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class ShoppingBag
    {
        public List<OrderLine> OrderLines { get; set; }

        public decimal TotalCost
        {
            get
            { //Bekijk alles in de lijst en tel het totaalbedrag op
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
}