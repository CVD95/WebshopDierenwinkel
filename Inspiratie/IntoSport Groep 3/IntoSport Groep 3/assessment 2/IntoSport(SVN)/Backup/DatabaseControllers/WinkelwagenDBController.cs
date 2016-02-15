using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntoSport.Models;
using MySql.Data.MySqlClient;

namespace IntoSport.DatabaseControllers
{
    public class WinkelwagenDBController : DatabaseController
    {
        public List<WinkelwagenItem> GetProductByGuid(string response)
        {
            List<WinkelwagenItem> winkelwagenItems = new List<WinkelwagenItem>();
            try
            {
                conn.Open();

                string selectQuery = @"select p.*, kwantiteit, maat, kleur from tempwinkelwagen tw
                    inner join product p on tw.productcode = p.productcode
                    where guid=@guid";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter guidParam = new MySqlParameter("guid", MySqlDbType.VarChar);

                guidParam.Value = response;

                cmd.Parameters.Add(guidParam);

                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    WinkelwagenItem winkelwagenItem = GetWinkelwagenItemFromDataReader(dataReader);
                    winkelwagenItems.Add(winkelwagenItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return winkelwagenItems;
        }
        public bool GetProduct(Producten product, string response)
        {

            try
            {
                conn.Open();

                string selectQuery = "select * from tempwinkelwagen where productcode=@productcode AND guid=@guid";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter productcodeParam = new MySqlParameter("productcode", MySqlDbType.Int32);
                MySqlParameter guidParam = new MySqlParameter("guid", MySqlDbType.VarChar);

                productcodeParam.Value = product.productCode;
                guidParam.Value = response;

                cmd.Parameters.Add(productcodeParam);
                cmd.Parameters.Add(guidParam);

                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                    return true;
                else
                    return false;

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
        }
        public bool GetTempProduct(Producten product, string maat, string kleur, string response)
        {
            try
            {
                conn.Open();

                string selectQuery = "select * from tempwinkelwagen where productcode=@productcode AND guid=@guid AND kleur=@kleur AND maat=@maat";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter productcodeParam = new MySqlParameter("productcode", MySqlDbType.Int32);
                MySqlParameter guidParam = new MySqlParameter("guid", MySqlDbType.VarChar);
                MySqlParameter kleurParam = new MySqlParameter("kleur", MySqlDbType.VarChar);
                MySqlParameter maatParam = new MySqlParameter("maat", MySqlDbType.VarChar);

                productcodeParam.Value = product.productCode;
                guidParam.Value = response;
                kleurParam.Value = kleur;
                maatParam.Value = maat;

                cmd.Parameters.Add(productcodeParam);
                cmd.Parameters.Add(guidParam);
                cmd.Parameters.Add(kleurParam);
                cmd.Parameters.Add(maatParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                    return true;
                else
                    return false;

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
        }
        public List<WinkelwagenItem> GetWinkelwagenProducten(string response)
        {
            List<WinkelwagenItem> winkelwagenItems = new List<WinkelwagenItem>();
            try
            {
                conn.Open();

                string selectQuery = "select p.*, kwantiteit, kleur, maat from tempwinkelwagen tw " +
                    "inner join product p on tw.productcode = p.productcode " +
                    " where guid=@guid";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter guidParam = new MySqlParameter("guid", MySqlDbType.VarChar);

                guidParam.Value = response;

                cmd.Parameters.Add(guidParam);

                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    WinkelwagenItem winkelwagenItem = GetWinkelwagenItemFromDataReader(dataReader);
                    winkelwagenItems.Add(winkelwagenItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return winkelwagenItems;
        }
        public void InsertTempWinkelwagenItem(Producten product, string maat, string kleur, string response)
        {
            try
            {
                conn.Open();

                string insertString = @"insert into tempWinkelwagen (guid, productcode, maat, kleur, eind_datum) " +
                                                       " values (@guid, @productcode, @maat, @kleur, @eind_datum)";

                //commando en prepared statement
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter guidParam = new MySqlParameter("@guid", MySqlDbType.VarChar);
                MySqlParameter productcodeParam = new MySqlParameter("@productcode", MySqlDbType.Int32);
                MySqlParameter kleurParam = new MySqlParameter("@kleur", MySqlDbType.VarChar);
                MySqlParameter einddatumParam = new MySqlParameter("@eind_datum", MySqlDbType.DateTime);
                MySqlParameter maatParam = new MySqlParameter("@maat", MySqlDbType.VarChar);

                // Get dateTime
                var now = DateTime.Now;

                //set parameters naar view Waarden
                guidParam.Value = response;
                productcodeParam.Value = product.productCode;
                kleurParam.Value = kleur;
                maatParam.Value = maat;
                einddatumParam.Value = now.AddMonths(1);

                //voeg parameters toe
                cmd.Parameters.Add(guidParam);
                cmd.Parameters.Add(productcodeParam);
                cmd.Parameters.Add(kleurParam);
                cmd.Parameters.Add(maatParam);
                cmd.Parameters.Add(einddatumParam);

                //prepare Execute Commit
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {// is er een fout?
                Console.Write("Temp Winkelwagen niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public void InsertFactuurEnBestellingDetail(Producten product, int kwantiteit, string maat, string kleur, int usr, string response, string keuze)
        {
            MySqlTransaction trans = null;
            int id = GetFactuurId() + 1; ;
            try
            {
                conn.Open();

                trans = conn.BeginTransaction();
                InsertFactuur(product, kwantiteit, maat, kleur, usr, response);
                InsertBestellingDetail(response, id, keuze);

                trans.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exceptie in Insert Factuur en Bestelling, Transactie mislukt");
                trans.Rollback();
                throw e;
            }
            finally
            {
                conn.Close();
            }
            
        }
        public void InsertFactuur(Producten product, int kwantiteit, string maat, string kleur, int usr, string response)
        {
            try
            { //open transactie en maak insert string en verbinding
                // conn.Open();

                string insertString = @"insert into factuur (datum, klant_code, product_code, totaalbedrag, kwantiteit, maat, kleur, guid) " +
                                                       " values (@datum, @klant_code, @product_code, @totaalbedrag, @kwantiteit, @maat, @kleur, @guid)";

                //command en prepared statement
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter datumParam = new MySqlParameter("@datum", MySqlDbType.Date);
                MySqlParameter klantcodeParam = new MySqlParameter("@klant_code", MySqlDbType.Int32);
                MySqlParameter productcodeParam = new MySqlParameter("@product_code", MySqlDbType.Int32);
                MySqlParameter totaalbedragParam = new MySqlParameter("@totaalbedrag", MySqlDbType.Double);
                MySqlParameter kwantiteitParam = new MySqlParameter("@kwantiteit", MySqlDbType.Int32);
                MySqlParameter maatParam = new MySqlParameter("@maat", MySqlDbType.VarChar);
                MySqlParameter kleurParam = new MySqlParameter("@kleur", MySqlDbType.VarChar);
                MySqlParameter guidParam = new MySqlParameter("@guid", MySqlDbType.VarChar);

                //set parameters naar view Waarden
                datumParam.Value = DateTime.Now;
                klantcodeParam.Value = usr;
                productcodeParam.Value = product.productCode;
                totaalbedragParam.Value = product.productVerkoopprijs * kwantiteit;
                kwantiteitParam.Value = kwantiteit;
                maatParam.Value = maat;
                kleurParam.Value = kleur;
                guidParam.Value = response;

                //voeg parameters toe
                cmd.Parameters.Add(datumParam);
                cmd.Parameters.Add(klantcodeParam);
                cmd.Parameters.Add(productcodeParam);
                cmd.Parameters.Add(totaalbedragParam);
                cmd.Parameters.Add(kwantiteitParam);
                cmd.Parameters.Add(maatParam);
                cmd.Parameters.Add(kleurParam);
                cmd.Parameters.Add(guidParam);

                //prepare Execute Commit
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {// is er een fout?
                Console.Write("Temp Winkelwagen niet toegevoegd: " + e);
                throw e;
            }
        }

        public void InsertBestellingDetail(string response, int id, string keuze)
        {
            try
            { //open transactie en maak insert string en verbinding
                //conn.Open();

                //                                                      GUID van Klant, Ophalen/Verzenden
                string insertString = @"insert into bestelling_detail (bestelling_id, factuur_code, bestelling_keuze)
                                                       values (@bestelling_id, @factuur_code, @bestelling_keuze)";

                //command en prepared statement
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter bestellingIdParam = new MySqlParameter("@bestelling_id", MySqlDbType.VarChar);
                MySqlParameter bestellingKeuzeParam = new MySqlParameter("@bestelling_keuze", MySqlDbType.VarChar);
                MySqlParameter factuurCodeParam = new MySqlParameter("@factuur_code", MySqlDbType.Int32);

                //set parameters
                bestellingIdParam.Value = response;
                bestellingKeuzeParam.Value = keuze;
                factuurCodeParam.Value = id;

                //voeg parameters toe
                cmd.Parameters.Add(bestellingIdParam);
                cmd.Parameters.Add(bestellingKeuzeParam);
                cmd.Parameters.Add(factuurCodeParam);

                //prepare Execute Commit
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {// is er een fout?
                Console.Write("Temp Winkelwagen niet toegevoegd: " + e);
                throw e;
            }
        }

        // Kwantiteit Updaten + 
        public void UpdateWinkelwagenItemKwantiteitPlus(int productcode, string maat, string kleur, string response)
        {
            try
            {
                string insertString = @"UPDATE tempwinkelwagen SET kwantiteit=kwantiteit+1 where guid=@guid AND productcode=@productcode AND maat=@maat AND kleur=@kleur";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter productcodeParam = new MySqlParameter("@productcode", MySqlDbType.Int32);
                MySqlParameter guidParam = new MySqlParameter("@guid", MySqlDbType.VarChar);
                MySqlParameter kleurParam = new MySqlParameter("@kleur", MySqlDbType.VarChar);
                MySqlParameter maatParam = new MySqlParameter("@maat", MySqlDbType.VarChar);

                guidParam.Value = response;
                productcodeParam.Value = productcode;
                kleurParam.Value = kleur;
                maatParam.Value = maat;

                cmd.Parameters.Add(guidParam);
                cmd.Parameters.Add(productcodeParam);
                cmd.Parameters.Add(kleurParam);
                cmd.Parameters.Add(maatParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Updaten winkelwagenItem niet gelukt: " + e);
                throw e;
            }

        }

        // Kwantiteit updaten -
        public void UpdateWinkelwagenItemKwantiteit(int productcode, string maat, string kleur, string response)
        {
            try
            {

                string insertString = @"UPDATE tempwinkelwagen SET kwantiteit=kwantiteit-1 where guid=@guid AND productcode=@productcode AND maat=@maat AND kleur=@kleur";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter productcodeParam = new MySqlParameter("@productcode", MySqlDbType.Int32);
                MySqlParameter guidParam = new MySqlParameter("@guid", MySqlDbType.VarChar);
                MySqlParameter maatParam = new MySqlParameter("@maat", MySqlDbType.VarChar);
                MySqlParameter kleurParam = new MySqlParameter("@kleur", MySqlDbType.VarChar);

                guidParam.Value = response;
                productcodeParam.Value = productcode;
                maatParam.Value = maat;
                kleurParam.Value = kleur;

                cmd.Parameters.Add(guidParam);
                cmd.Parameters.Add(productcodeParam);
                cmd.Parameters.Add(maatParam);
                cmd.Parameters.Add(kleurParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Updaten winkelwagenItem niet gelukt: " + e);
                throw e;
            }

        }

        public void VerwijderKwantiteit(int productcode, string maat, string kleur, string response)
        {
            try
            {

                string insertString = @"delete from tempwinkelwagen where guid=@guid AND productcode=@productcode AND kwantiteit=@kwantiteit AND maat=@maat AND kleur=@kleur";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter guidParam = new MySqlParameter("@guid", MySqlDbType.VarChar);
                MySqlParameter productcodeParam = new MySqlParameter("productcode", MySqlDbType.Int32);
                MySqlParameter kwantiteitParam = new MySqlParameter("kwantiteit", MySqlDbType.Int32);
                MySqlParameter maatParam = new MySqlParameter("maat", MySqlDbType.VarChar);
                MySqlParameter kleurParam = new MySqlParameter("kleur", MySqlDbType.VarChar);

                guidParam.Value = response;
                productcodeParam.Value = productcode;
                kwantiteitParam.Value = 0;
                maatParam.Value = maat;
                kleurParam.Value = kleur;

                cmd.Parameters.Add(guidParam);
                cmd.Parameters.Add(productcodeParam);
                cmd.Parameters.Add(kwantiteitParam);
                cmd.Parameters.Add(maatParam);
                cmd.Parameters.Add(kleurParam);

                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Product niet verwijderd: " + e);
                throw e;
            }
        }

        public void VerwijderProduct(int productcode, string maat, string kleur, string response)
        {
            try
            {
                conn.Open();

                string insertString = @"delete from tempwinkelwagen where guid=@guid AND productcode=@productcode AND maat=@maat AND kleur=@kleur OR kwantiteit=0";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter guidParam = new MySqlParameter("@guid", MySqlDbType.VarChar);
                MySqlParameter productcodeParam = new MySqlParameter("@productcode", MySqlDbType.Int32);
                MySqlParameter maatParam = new MySqlParameter("@maat", MySqlDbType.VarChar);
                MySqlParameter kleurParam = new MySqlParameter("@kleur", MySqlDbType.VarChar);

                guidParam.Value = response;
                productcodeParam.Value = productcode;
                maatParam.Value = maat;
                kleurParam.Value = kleur;

                cmd.Parameters.Add(guidParam);
                cmd.Parameters.Add(productcodeParam);
                cmd.Parameters.Add(maatParam);
                cmd.Parameters.Add(kleurParam);

                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Product niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void VerwijderAlleProducten(string response)
        {

            try
            {
                string insertString = @"delete from tempwinkelwagen where guid=@guid";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter guidParam = new MySqlParameter("@guid", MySqlDbType.VarChar);

                guidParam.Value = response;

                cmd.Parameters.Add(guidParam);

                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("WinkelwagenItem niet verwijderd: " + e);
                throw e;
            }
        }

        public void VerwijderAlleTempWinkelwagenItemPassedDate()
        {
            try
            {
                conn.Open();

                string insertString = @"delete from tempwinkelwagen WHERE eind_datum < NOW()";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);

                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }

        }
        public double GetTotal(string response)
        {
            double total = 0;
            try
            {
                conn.Open();

                string selectQuery = "SELECT p.verkoopprijs, tw.kwantiteit FROM tempwinkelwagen tw " +
                    "join product p on tw.productcode = p.productcode where guid=@guid";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter guidParam = new MySqlParameter("guid", MySqlDbType.VarChar);

                guidParam.Value = response;

                cmd.Parameters.Add(guidParam);

                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    total = total + GetWinkelwagenTotaalFromDataReader(dataReader);
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

            return total;
        }

        public int GetFactuurId()
        {
            int id = 0;

            try
            {
                conn.Open();
                string getQuery = @"select max(factuur_code) as factuur_code from factuur";

                MySqlCommand cmd = new MySqlCommand(getQuery, conn);

                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    id = GetFactuurIdFromDataReader(dataReader);
                }
            }
            catch (Exception e)
            {
                // Moet naar error page, niet alleen throw e
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return id;
        }

        //Store methodes via een list met <Action>, en execute ze in MySQLTransaction
        public void executeMethodMetMysqlTrans(List<Action> list)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                // Execute methodes in de list in volgorde via een loop.
                foreach (var action in list)
                {
                    action.Invoke();
                }

                // Commit data
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }
            finally
            {
                conn.Close();
            }

        }

        public void executeMethodMetConn(List<Action> list)
        {
            connOpen();

            foreach (var action in list)
            {
                action.Invoke();
            }

            connClose();
        }
        public void connOpen()
        {
            // Hacky, moet beter!
            conn.Open();
        }
        public void connClose()
        {
            // Hacky, moet beter!
            conn.Close();
        }

    }
}
