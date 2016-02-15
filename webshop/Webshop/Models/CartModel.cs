using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using Webshop.Database;

namespace Webshop.Models
{
    public class CartModel
    {
        public List<CartEntry> Items { get; private set; } = new List<CartEntry>();

        public int TotalDifferentItems { get { return Items.Count; } }
        public decimal TotalCost
        {
            get
            { //Bekijk alles in de lijst en tel het totaalbedrag op
                decimal returnValue = 0;

                for (int i = 0; i < Items.Count; i++)
                {
                    returnValue += Items.ElementAt(i).Cost;
                }

                return returnValue;
            }
        }

        /// <summary>
        /// saves this CartModel to the database.
        /// If successfull, this CartModel will be empty.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool Buy(User customer)
        { //Nog niet geimplementeerd
            throw new NotImplementedException("save everything to database");
            /*
            using (DatabaseQuerry querry = new DatabaseQuerry())
            {
                if(querry.CreateOrder(cartModel, customer))
                {
                    Items = null;                    
                }
            }
            */
        }

    }

    public class CartEntry
    {
        public Product Product { get; set; }
        public ulong ItemCount { get; set; }
        public decimal Cost { get { return Product.Price * ItemCount; } } //Kosten is prijs van het prouct maal het aantal
                                                                          //Winkelwagen heeft producten aantal en Cost
        public CartEntry(ulong itemId, ulong itemCount)
        {
            using (DatabaseQuery querry = new DatabaseQuery())
            {
                Product product = querry.GetProduct((ulong)itemId);
                Contract.Assert(product != null); //Assert een nieuw product in de lijst
            }
            ItemCount = itemCount; //Tel de items
        }

        public CartEntry(Product product, ulong itemCount)
        {
            Contract.Assert(product != null);
            Product = product;
            ItemCount = itemCount;
        }

    }
}
