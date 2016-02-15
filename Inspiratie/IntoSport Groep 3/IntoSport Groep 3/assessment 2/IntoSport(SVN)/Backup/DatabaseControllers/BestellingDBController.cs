using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using IntoSport.Models;

namespace IntoSport.DatabaseControllers
{
    public class BestellingDBController : DatabaseController
    {
        public List<Producten> GetBestelling()
        {
            List<Producten> producten = new List<Producten>();
            try
            {
                conn.Open();

                string selectQuery = "select productcode, naam, merk, producttype, inkoopprijs, verkoopprijs, voorraad, p.aanbiedingscode, omschrijving from product p join aanbieding a on p.aanbiedingscode = a.aanbiedingscode where p.aanbiedingscode > 0";

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
                Console.Write("Ophalen van aanbiedingen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return producten;
        }

        public BestellingOverzicht GetBestelling(int bestellings_code)
        {
            BestellingOverzicht bestelling = null;

            try
            {
                conn.Open();

                string selectQuery = "select * from factuur where factuur_code=@factuur_code";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter bestellingcodeParameter = new MySqlParameter("factuur_code", MySqlDbType.Int32);
                bestellingcodeParameter.Value = bestellings_code;
                cmd.Parameters.Add(bestellingcodeParameter);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    bestelling = GetBestellingFromDataReader(dataReader);
                }

            }
            catch (Exception e)
            {

                Console.Write("Ophalen van Bestelling mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return bestelling;
        }

        public void UpdateBestelling(BestellingOverzicht bestelling)
        {
            try
            {
                conn.Open();
                string insertString = @"UPDATE factuur SET status=@status WHERE factuur_code=@factuur_code";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter bestellingscodeParam = new MySqlParameter("@factuur_code", MySqlDbType.Int32);
                MySqlParameter statusParam = new MySqlParameter("@status", MySqlDbType.VarChar);


                bestellingscodeParam.Value = bestelling.Factuur_code;
                statusParam.Value = bestelling.Status;

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

        public void AnnulerenBestelling(int bestellingscode)
        {
            try
            {
                conn.Open();
                
                string insertString = @"UPDATE factuur " +
                                        "SET status=@status " +
                                         "WHERE factuur_code=@factuur_code";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter bestellingscodeParam = new MySqlParameter("@factuur_code", MySqlDbType.Int32);
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
    }
}
