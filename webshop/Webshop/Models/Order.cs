﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webshop.Enums;
using Webshop.Functions;

namespace Webshop.Models
{
    public class Order : TotalCostCalculator
    {
        public ulong Id { get; set; }
        public User User { get; set; }
        public DateTime DTime { get; set; }
        public override List<OrderLine> OrderLines { get; set; }
        public OrderStatus Status { get; set; }
    }
}