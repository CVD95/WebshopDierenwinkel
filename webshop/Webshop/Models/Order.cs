using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webshop.Enums;

namespace Webshop.Models
{
    public class Order
    {
        public ulong Id { get; set; }
        public User User { get; set; }
        public DateTime DTime { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public OrderStatus Status { get; set; }
    }
}