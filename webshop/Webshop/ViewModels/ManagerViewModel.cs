using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webshop.Models;

namespace Webshop.ViewModels
{
    public class ManagerViewModel
    {
        public List<Order> Orders { get; set; }
        public List<OrderLine> Orderlines { get; set; }
        public List<OrderLine> OrderlinesAsc { get; set; }

        public decimal ProfitAllTime()
        {
            decimal profit = 0;
            foreach(OrderLine o in Orderlines)
            {
                profit += ((o.Product.Price - o.Product.BuyPrice)* o.Amount);
            }
            return profit;
        }

        public decimal ProfitThisMonth()
        {
            decimal profit = 0;
            foreach (Order o in Orders)
            {
                double days = (DateTime.Now - o.DTime).TotalDays;
                if (days < 30.4)
                {
                    foreach (OrderLine ol in o.OrderLines)
                    {
                        profit += ((ol.Product.Price - ol.Product.BuyPrice) * ol.Amount);
                    }
                }
            }
            return profit;
        }
    }
}