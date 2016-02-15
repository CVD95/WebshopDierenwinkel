using System.Collections.Generic;
using Webshop.Database;

namespace Webshop.Models
{
    public class SearchResultModel
    {
        public List<Product> Products { get; set; }
        //Zoekresultaat kan een aantal producten hebben
        public SearchResultModel(string searchFor)
        {
            if (string.IsNullOrWhiteSpace(searchFor))
            { //Er kan niet worden gezocht op een Whiet SPace
                throw new System.Exception("Er kan niet worden gezocht voor een leeg zoekresultaat of een spatiebalkteken");
            }
            using (DatabaseQuery querry = new DatabaseQuery())
            { //Haal alle producten op waar naar is gezocht met de SearchFor Variabele
                Products = querry.GetProducts(searchFor);
            }
        }
    }
}