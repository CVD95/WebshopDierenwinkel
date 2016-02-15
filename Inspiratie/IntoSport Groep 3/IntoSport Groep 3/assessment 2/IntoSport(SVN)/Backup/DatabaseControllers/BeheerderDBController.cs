using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using IntoSport.Models;

namespace IntoSport.DatabaseControllers
{
    public class BeheerderDBController : DatabaseController
    {
        public BeheerderDBController() { }


        public List<Klant> GetKlant()
        {
            List<Klant> klanten = new List<Klant>();
            try
            {
                conn.Open();
                
                string selectQuery = "select * from klant";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Klant klant = GetKlantFromDataReader(dataReader);
                    klanten.Add(klant);
                }
            }
            catch(Exception e)
            {
                Console.Write("Ophalen van klanten mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
            
            return klanten;
        }

        public Klant GetKlant(int klantcode)
        {
            Klant klant = null;
            try
            {
                conn.Open();

                string selectQuery = "select * from klant where klantcode = @klantcode";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);


                MySqlParameter klantcodeParameter = new MySqlParameter("klantcode", MySqlDbType.Int32);
                klantcodeParameter.Value = klantcode;
                cmd.Parameters.Add(klantcodeParameter);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    klant = GetKlantFromDataReader(dataReader);

                }

            }
            catch (Exception e)
            {
                Console.Write("Ophalen van klanten mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return klant;
        }

        public List<BestellingOverzicht> GetBestelling()
        {
            List<BestellingOverzicht> bestellingen = new List<BestellingOverzicht>();
            try
            {
                conn.Open();

                string selectQuery = "select * from factuur";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    BestellingOverzicht bestelling = GetBestellingFromDataReader(dataReader);
                    bestellingen.Add(bestelling);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van bestellingen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return bestellingen;
        }

        public Producten GetProduct(int productcode)
        {
            Producten product = null;

            try
            {
                conn.Open();

                string selectQuery = "select * from product where productcode=@productcode";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter productcodeParameter = new MySqlParameter("productcode", MySqlDbType.Int32);
                productcodeParameter.Value = productcode;
                cmd.Parameters.Add(productcodeParameter);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    product = getProductenFromDataReader(dataReader);
                }

            }
            catch (Exception e)
            {

                Console.Write("Ophalen van Product mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return product;
        }

        public List<Sport> GetSporten()
        {
            List<Sport> sporten = new List<Sport>();
            try
            {
                conn.Open();

                string selectQuery = "SELECT * FROM sport;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Sport sport = getSportenFromDataReader(dataReader);
                    sporten.Add(sport);
                }

            }
            catch (Exception e)
            {
                Console.Write("Ophalen van Producten mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return sporten;
        }

        public List<Producten> GetProducten()
        {
            List<Producten> producten = new List<Producten>();
            try
            {
                conn.Open();

                string selectQuery = "SELECT * FROM product;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Producten product = getProductenFromDataReader(dataReader);
                    producten.Add(product);
                }

            }
            catch (Exception e)
            {
                Console.Write("Ophalen van Producten mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return producten;
        }

        public void InsertProduct(Producten product)
        {
            try
            { //open transactie en maak insert string en verbinding
                conn.Open();

                string insertString = @"insert into product (naam, merk, inkoopprijs, verkoopprijs, voorraad , aanbiedingscode  , producttype) " +
                                                       " values (@naam, @merk,  @inkoopprijs, @verkoopprijs, @voorraad , @aanbiedingscode , @producttype)";

                //commando en prepared statement
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter naamparameter = new MySqlParameter("@naam", MySqlDbType.String);
                MySqlParameter merkparameter = new MySqlParameter("@merk", MySqlDbType.String);
                MySqlParameter inkoopprijsparameter = new MySqlParameter("@inkoopprijs", MySqlDbType.Double);
                MySqlParameter verkoopprijsparameter = new MySqlParameter("@verkoopprijs", MySqlDbType.Double);
                MySqlParameter voorraadparameter = new MySqlParameter("@voorraad", MySqlDbType.Int32);
                MySqlParameter aanbiedingscodeparameter = new MySqlParameter("@aanbiedingscode", MySqlDbType.Int32);
                MySqlParameter producttypeparameter = new MySqlParameter("@producttype", MySqlDbType.String);

                //set parameters naar view Waarden
                naamparameter.Value = product.productNaam;
                merkparameter.Value = product.productMerk;
                inkoopprijsparameter.Value = product.productInkoopprijs;
                verkoopprijsparameter.Value = product.productVerkoopprijs;
                voorraadparameter.Value = product.productVoorraad;
                aanbiedingscodeparameter.Value = product.productAanbiedingscode;
                producttypeparameter.Value = product.productType;

                //voeg parameters toe
                cmd.Parameters.Add(naamparameter);
                cmd.Parameters.Add(merkparameter);
                cmd.Parameters.Add(inkoopprijsparameter);
                cmd.Parameters.Add(verkoopprijsparameter);
                cmd.Parameters.Add(voorraadparameter);
                cmd.Parameters.Add(aanbiedingscodeparameter);
                cmd.Parameters.Add(producttypeparameter);

                //prepare Execute Commit
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {// is er een fout?
                Console.Write("Product niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateProduct(Producten product)
        {
            try
            {
                conn.Open();

                string insertString = @"UPDATE product " +
                                        "SET naam=@productnaam, merk=@productmerk, inkoopprijs=@productinkoopprijs, verkoopprijs=@productverkoopprijs, voorraad=@productvoorraad, aanbiedingscode=@productaanbiedingscode, producttype=@producttype " +
                                         "WHERE productcode=@productId";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter productcodeParameter = new MySqlParameter("@productId", MySqlDbType.Int32);
                MySqlParameter productnaamParameter = new MySqlParameter("@productnaam", MySqlDbType.String);
                MySqlParameter productmerkParameter = new MySqlParameter("@productmerk", MySqlDbType.String);
                MySqlParameter productinkoopprijsParameter = new MySqlParameter("@productinkoopprijs", MySqlDbType.Double);
                MySqlParameter productverkoopprijsParameter = new MySqlParameter("@productverkoopprijs", MySqlDbType.Double);
                MySqlParameter productvoorraadParameter = new MySqlParameter("@productvoorraad", MySqlDbType.Int32);
                MySqlParameter productaanbiedingscodeParameter = new MySqlParameter("@productaanbiedingscode", MySqlDbType.Int32);
                MySqlParameter productmaatParameter = new MySqlParameter("@productmaat", MySqlDbType.String);
                MySqlParameter producttypeParameter = new MySqlParameter(@"producttype", MySqlDbType.String);


                productnaamParameter.Value = product.productNaam;
                productcodeParameter.Value = product.productCode;
                productmerkParameter.Value = product.productMerk;
                productinkoopprijsParameter.Value = product.productInkoopprijs;
                productverkoopprijsParameter.Value = product.productVerkoopprijs;
                productvoorraadParameter.Value = product.productVoorraad;
                productaanbiedingscodeParameter.Value = product.productAanbiedingscode;
                //productmaatParameter.Value = product.productMaat;
                producttypeParameter.Value = product.productType;

                cmd.Parameters.Add(productcodeParameter);
                cmd.Parameters.Add(productnaamParameter);
                cmd.Parameters.Add(productmerkParameter);
                cmd.Parameters.Add(productinkoopprijsParameter);
                cmd.Parameters.Add(productverkoopprijsParameter);
                cmd.Parameters.Add(productvoorraadParameter);
                cmd.Parameters.Add(productaanbiedingscodeParameter);
                cmd.Parameters.Add(productmaatParameter);
                cmd.Parameters.Add(producttypeParameter);


                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Updaten product niet gelukt: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

        }

        public void VerwijderProduct(int productcode)
        {
            try
            {
                conn.Open();
                string insertString = @"delete from product where productcode=@id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter idCodeParam = new MySqlParameter("@id", MySqlDbType.Int32);

                idCodeParam.Value = productcode;

                cmd.Parameters.Add(idCodeParam);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Evenement niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public void AnnulerenBestelling(int bestellingscode)
        {

            try
            {
                conn.Open();
                
                string insertString = @"UPDATE bestelling " +
                                        "SET status=@status" +
                                         "WHERE bestellings_code=@bestellings_code";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter bestellingscodeParam = new MySqlParameter("@bestellings_code", MySqlDbType.Int32);
                MySqlParameter statusParam = new MySqlParameter("@status", MySqlDbType.Enum);

                bestellingscodeParam.Value = bestellingscode;
                statusParam.Value = "Geannulleerd";

                cmd.Parameters.Add(bestellingscodeParam);
                cmd.Parameters.Add(statusParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Updaten bestelling niet gelukt: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public void VerwijderBestelling(int bestellingscode)
        {
            try
            {
                conn.Open();

                string insertString = @"delete from bestelling where bestellings_code=@bestellingscode";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter bestellingscodeParam = new MySqlParameter("@bestellingscode", MySqlDbType.Int32);

                bestellingscodeParam.Value = bestellingscode;

                cmd.Parameters.Add(bestellingscodeParam);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Evenement niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void VerwijderBestellingOphalen(int bestellingscode)
        {
            try
            {
                conn.Open();
                
                string insertString = @"delete from ophalen_bestelling where bestellings_code=@bestellingscode";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter bestellingscodeParam = new MySqlParameter("@bestellingscode", MySqlDbType.Int32);

                bestellingscodeParam.Value = bestellingscode;

                cmd.Parameters.Add(bestellingscodeParam);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Bestelling niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void VerwijderVersturenBestelling(int bestellingscode)
        {
            try
            {
                conn.Open();
                
                string insertString = @"delete from versturen_bestelling where bestellings_code=@bestellingscode";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter bestellingscodeParam = new MySqlParameter("@bestellingscode", MySqlDbType.Int32);

                bestellingscodeParam.Value = bestellingscode;

                cmd.Parameters.Add(bestellingscodeParam);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Bestelling niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
