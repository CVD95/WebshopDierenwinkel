using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using IntoSport.Models;

namespace IntoSport.DatabaseControllers
{
    public class DatabaseController : Controller
    {

        protected MySqlConnection conn;

        public DatabaseController()
        {
            conn = new MySqlConnection("Server=localhost;Port=3306;Database=intosport1;Uid=root;Pwd=admin;");
        }

        // Haal overzicht voor de Manager binnen
        protected Manager GetOverzichtFromDataReader(MySqlDataReader dataReader)
        {
            String naam = dataReader.GetString("naam");
            double inkomsten = dataReader.GetDouble("omzet");
            double winst = dataReader.GetDouble("winst");


            Manager login = new Manager { Productnaam = naam, Omzet = inkomsten, Winst = winst };
            return login;
        }

        protected Manager GetOverzichtPerMaandFromDataReader(MySqlDataReader dataReader)
        {
            String naam = dataReader.GetString("naam");
            int Maand = dataReader.GetInt32("maand");
            int Jaar = dataReader.GetInt32("jaar");
            double inkomsten = dataReader.GetDouble("omzet");
            double winst = dataReader.GetDouble("winst");

            Manager login = new Manager { Productnaam = naam, Maand = Maand, Jaar = Jaar, Omzet = inkomsten, Winst = winst };
            return login;
        }

        protected WinkelwagenItem GetWinkelwagenItemFromDataReader(MySqlDataReader dataReader)
        {
            int productcode = dataReader.GetInt32("productcode");
            string productnaam = dataReader.GetString("naam");
            string productmerk = dataReader.GetString("merk");
            int productaanbiedingscode = dataReader.GetInt32("aanbiedingscode");
            double productinkoopprijs = dataReader.GetDouble("inkoopprijs");
            string producttype = dataReader.GetString("producttype");
            double productverkoopprijs = dataReader.GetDouble("verkoopprijs");
            int productvoorraad = dataReader.GetInt32("voorraad");
            string productomschrijving = dataReader.GetString("omschrijving");
            int kwantiteit = dataReader.GetInt32("kwantiteit");
            string kleur = dataReader.GetString("kleur");
            string maat = dataReader.GetString("maat");

            Producten product = new Producten
            {
                productCode = productcode,
                productNaam = productnaam,
                productMerk = productmerk,
                productAanbiedingscode = productaanbiedingscode,
                productInkoopprijs = productinkoopprijs,
                productType = producttype,
                productVerkoopprijs = productverkoopprijs,
                productVoorraad = productvoorraad,
                productOmschrijving = productomschrijving
            };
            WinkelwagenItem item = new WinkelwagenItem(product, kwantiteit, maat, kleur);
            return item;
        }

        // Get KlantId van klant Gebruikersnaam
        protected Int32 GetKlantIdFromDataReader(MySqlDataReader dataReader)
        {
            int klantcode = dataReader.GetInt32("klantcode");

            return klantcode;
        }

        // Get Email van klant
        protected String GetKlantEmailFromDataReader(MySqlDataReader dataReader)
        {
            string klantEmail = dataReader.GetString("email");

            return klantEmail;
        }
        // Haal overzicht over de Klant binnen
        protected Klant GetKlantFromDataReader(MySqlDataReader dataReader)
        {
            int klantId = dataReader.GetInt32("klantcode");
            string klantGebruikersnaam = dataReader.GetString("gebruikersnaam");
            string klantWachtwoord = dataReader.GetString("wachtwoord");
            string klantNaam = dataReader.GetString("naam");
            string klantAdres = dataReader.GetString("adres");
            string klantWoonplaats = dataReader.GetString("woonplaats");
            string klantTelefoonnummer = dataReader.GetString("telefoonnummer");
            string klantEmail = dataReader.GetString("email");
            string klantRechten = dataReader.GetString("rechten");
            DateTime klantDatum_inschrijving = dataReader.GetDateTime("datum_inschrijving");

            if (dataReader["gebruikersnaam"] == DBNull.Value)
            {
                klantGebruikersnaam = "";
            }
            if (dataReader["wachtwoord"] == DBNull.Value)
            {
                klantWachtwoord = "";
            }

            Klant klant = new Klant { Id = klantId, Gebruikersnaam = klantGebruikersnaam, Wachtwoord = klantWachtwoord, Naam = klantNaam, 
                                      Adres = klantAdres, Woonplaats = klantWoonplaats, Telefoonnummer = klantTelefoonnummer,
                                      Email = klantEmail, Rechten = klantRechten};
            return klant;
        }

        // Haal overzicht over de Bestellingen binnen BEHEERDER
        protected BestellingOverzicht GetBestellingFromDataReader(MySqlDataReader dataReader)
        {
            int factuurCode = dataReader.GetInt32("factuur_code");
            string bestellingStatus = dataReader.GetString("status");
            float bestellingTotaal = dataReader.GetFloat("totaalbedrag");
            int klantCode = dataReader.GetInt32("klant_code");
            int productCode = dataReader.GetInt32("product_code");
            string guid = dataReader.GetString("guid");

            BestellingOverzicht bestellingOverzicht = new BestellingOverzicht
            {
                Factuur_code = factuurCode, 
                Status = bestellingStatus, 
                Totaal = bestellingTotaal, 
                Klant_code = klantCode,
                Product_code = productCode
            };
            return bestellingOverzicht;
        }

        protected Sport getSportenFromDataReader(MySqlDataReader dataReader)
        {
            int sportSportcode = dataReader.GetInt32("sportcode");
            string sportNaam = dataReader.GetString("naam");
            string sportType = dataReader.GetString("type");

            Sport sportOverzicht = new Sport
            {
                sportcode = sportSportcode,
                naam = sportNaam,
                type = sportType,
            };
            return sportOverzicht;
        }

        protected Producten getProductenFromDataReader(MySqlDataReader dataReader)
        {
            int productcode = dataReader.GetInt32("productcode");
            string productnaam = dataReader.GetString("naam");
            string productmerk = dataReader.GetString("merk");
            int productaanbiedingscode = dataReader.GetInt32("aanbiedingscode");
            double productinkoopprijs = dataReader.GetDouble("inkoopprijs");
            string producttype = dataReader.GetString("producttype");
            double productverkoopprijs = dataReader.GetDouble("verkoopprijs");
            int productvoorraad = dataReader.GetInt32("voorraad");
            string productomschrijving = dataReader.GetString("omschrijving");

            Producten product = new Producten { productCode = productcode, productNaam = productnaam, productMerk = productmerk, 
                                                productAanbiedingscode = productaanbiedingscode, productInkoopprijs = productinkoopprijs, 
                                                productType = producttype, productVerkoopprijs = productverkoopprijs, 
                                                productVoorraad = productvoorraad, productOmschrijving = productomschrijving };
            return product;
        }

        protected int GetProductIdFromDataReader(MySqlDataReader dataReader)
        {
            int productcode = dataReader.GetInt32("productcode");

            return productcode;
        }

        protected int GetFactuurIdFromDataReader(MySqlDataReader dataReader)
        {
            int factuurcode = dataReader.GetInt32("factuur_code");

            return factuurcode;
        }
        // Haal overzicht over de Bestellingen binnen KLANT
        protected KlantBestellingOverzicht GetKlantBestellingFromDataReader(MySqlDataReader dataReader)
        {
            int bestellingBestellingscode = dataReader.GetInt32("factuur_code");
            string bestellingProductNaam = dataReader.GetString("naam");
            float bestellingTotaal = dataReader.GetFloat("totaalbedrag");
            string bestellingStatus = dataReader.GetString("status");

            KlantBestellingOverzicht bestellingOverzicht = new KlantBestellingOverzicht
            {
                FactuurCode = bestellingBestellingscode,
                Naam = bestellingProductNaam,
                Totaal = bestellingTotaal,
                Status = bestellingStatus
            };
            return bestellingOverzicht;
        }
        /*
        // Haal overzicht over de Klant binnen
        protected Producten GetProductenFromDataReader(MySqlDataReader dataReader)
        {
            int productDRProductcode = dataReader.GetInt32("productcode");
            string productDRNaam = dataReader.GetString("naam");
            string productDRMerk = dataReader.GetString("merk");
            string productDRMaat = dataReader.GetString("maat");
            string productDRtype = dataReader.GetString("producttype");
            double productDRInkoopprijs = dataReader.GetDouble("inkoopprijs");
            double productDRVerkoopprijs = dataReader.GetDouble("verkoopprijs");
            int productDRVoorraad = dataReader.GetInt32("voorraad");
            int productDRAanbiedingscode = dataReader.GetInt32("aanbiedingscode");
            string productDRomschrijving = dataReader.GetString("omschrijving");

            Producten producten = new Producten
            {
                productCode = productDRProductcode,
                productNaam = productDRNaam,
                productMerk = productDRMerk,
                productMaat = productDRMaat,
                productType = productDRtype,
                productInkoopprijs = productDRInkoopprijs,
                productVerkoopprijs = productDRVerkoopprijs,
                productVoorraad = productDRVoorraad,
                productAanbiedingscode = productDRAanbiedingscode,
                productOmschrijving = productDRomschrijving
            };
            return producten;
        }
        */

        protected CategorieModel getCategorieFromDataReader(MySqlDataReader dataReader)
        {
            int Id = dataReader.GetInt32("categorie_Id");
            int Sport = dataReader.GetInt32("sport");
            int Product = dataReader.GetInt32("product");
            String Type = dataReader.GetString("producttype");
            CategorieModel Categorie = new CategorieModel { id = Id, sport = Sport, product = Product, type = Type };

            return Categorie;
        }

        protected Aanbieding GetAanbiedingFromDataReader(MySqlDataReader dataReader)
        {
            String Naam = dataReader.GetString("naam");
            int Aanbiedingscode = dataReader.GetInt32("aanbiedingscode");
            DateTime Geldig_tot = dataReader.GetDateTime("geldig_tot");
            int Kortingspercentage = dataReader.GetInt32("kortingspercentage");

            Aanbieding aanbieding = new Aanbieding { naam = Naam, aanbiedingscode = Aanbiedingscode, geldig_tot = Geldig_tot, kortingspercentage = Kortingspercentage  };
            return aanbieding;
        }
        protected Aanbieding GetAanbiedingAangepastFromDataReader(MySqlDataReader dataReader)
        {
            int Aanbiedingscode = dataReader.GetInt32("aanbiedingscode");
            String Naam = dataReader.GetString("naam");
            int Kortingspercentage = dataReader.GetInt32("kortingspercentage");

            Aanbieding aanbieding = new Aanbieding { aanbiedingscode = Aanbiedingscode, naam =  Naam, kortingspercentage = Kortingspercentage};
            return aanbieding;
        }

        protected MaatModel getMatenFromDataReader(MySqlDataReader dataReader)
        {
            int Maatcode = dataReader.GetInt32("id");
            String Maat = dataReader.GetString("maat");
            String Grootte = dataReader.GetString("grootte");
            String Kledingstuk = dataReader.GetString("kledingstuk");

            MaatModel maat = new MaatModel { id = Maatcode, maatcategorie = Maat, grootte = Grootte, kledingstuk = Kledingstuk };
            return maat;
        }

        protected string getMaatGrootteFromDataReader(MySqlDataReader dataReader)
        {
            string Grootte = dataReader.GetString("grootte");

            return Grootte;
        }
        protected double GetWinkelwagenTotaalFromDataReader(MySqlDataReader dataReader)
        {
            double verkoopprijs = dataReader.GetDouble("verkoopprijs");
            double kwantiteit = dataReader.GetDouble("kwantiteit");

            return verkoopprijs * kwantiteit;
        }

        // Convert Timestamp naar DateTime
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
