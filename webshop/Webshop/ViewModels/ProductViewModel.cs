using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop.Models;

namespace Webshop.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public SelectList Categories { get; set; }

        [Range(1, ulong.MaxValue, ErrorMessage="Het is verplicht om een categorie te selecteren.")]
        public ulong SelectedCategoryId { get; set; }
    }
}