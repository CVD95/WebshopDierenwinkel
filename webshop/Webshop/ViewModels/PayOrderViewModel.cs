using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop.Models;

namespace Webshop.ViewModels
{
    public class PayOrderViewModel
    {
        public Order Order { get; set; }
        public SelectList PaymentOptions { get; set; }

        [Range(1, ulong.MaxValue, ErrorMessage = "Het is verplicht om een categorie te selecteren.")]
        public ulong SelectedPaymentId { get; set; }
    }
}