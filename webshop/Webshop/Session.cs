using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webshop.Models;

namespace Webshop
{
    public class Session
    {
        public bool LoggedIn { get; set; } //default is false
        public User User { get; set; }

        public ShoppingBag ShoppingBag {get; set;}

        public Session()
        {
            ShoppingBag = new ShoppingBag();
            ShoppingBag.OrderLines = new List<OrderLine>();
        }
    }
}