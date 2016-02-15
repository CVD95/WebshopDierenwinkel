using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webshop.Models;

namespace Webshop.ViewModels
{
    public class ManagerViewModel
    {
        public List<OrderLine> Orderlines{ get; set; }
        public List<OrderLine> OrderlinesAsc { get; set; }
        public decimal Profit()
        {
            decimal profit = 0;
            foreach(OrderLine o in Orderlines)
            {
                profit += ((o.Product.Price - o.Product.BuyPrice)* o.Amount);
            }
            return profit;
        }
    }
}