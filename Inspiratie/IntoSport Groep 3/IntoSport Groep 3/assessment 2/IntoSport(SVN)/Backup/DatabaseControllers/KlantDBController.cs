using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntoSport.Models;
using MySql.Data.MySqlClient;


namespace IntoSport.DatabaseControllers
{
    public class KlantDBController : DatabaseController
    {
        public Klant GetKlantId(int klantId)
        {
            Klant klant = null;
            try
            {
                conn.Open();

                string selectQueryStudent = @"select * from klant where klantcode = @klantid";
                MySqlCommand cmd = new MySqlCommand(selectQueryStudent, conn);

                MySqlParameter klantIdParam = new MySqlParameter("klantid", MySqlDbType.Int32);
                klantIdParam.Value = klantId;
                cmd.Parameters.Add(klantIdParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    klant = GetKlantFromDataReader(dataReader);
                }

            }
            catch (Exception e)
            {
                Console.Write("Klant niet opgehaald: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return klant;
        }

        public Int32 GetAlleenKlantId(string usr)
        {
            int klantId = 0;
            try
            {
                conn.Open();

                string selectQueryStudent = @"select klantcode from klant where gebruikersnaam = @gebruikersnaam";
                MySqlCommand cmd = new MySqlCommand(selectQueryStudent, conn);

                MySqlParameter klantIdParam = new MySqlParameter("gebruikersnaam", MySqlDbType.VarChar);
                klantIdParam.Value = usr;
                cmd.Parameters.Add(klantIdParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    klantId = GetKlantIdFromDataReader(dataReader);
                }
            }
            catch (Exception e)
            {
                Console.Write("Klant niet opgehaald: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return klantId;
        }

        public string GetKlantEmail(int usr)
        {
            string email = "";
            try
            {
                conn.Open();

                string selectQuery = "select email from klant where klantcode = @klantcode";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter klantcodeParam = new MySqlParameter("@klantcode", MySqlDbType.Int32);

                klantcodeParam.Value = usr;

                cmd.Parameters.Add(klantcodeParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    email = GetKlantEmailFromDataReader(dataReader);
                }
                return email;
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

        public string GetKlantEmail(string usr)
        {
            string email = "";
            try
            {
                conn.Open();

                string selectQuery = "select email from klant where gebruikersnaam = @username";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter usernameParam = new MySqlParameter("@username", MySqlDbType.VarChar);

                usernameParam.Value = usr;

                cmd.Parameters.Add(usernameParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    email = GetKlantEmailFromDataReader(dataReader);
                }
                return email;
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

        public String GetKlantEmailMetBestelling(int factuur_code)
        {
            string email = "";
            try
            {
                conn.Open();

                string selectQueryStudent = @"select email from klant join factuur on factuur.klant_code = klant.klantcode where factuur_code=factuur_code";
                MySqlCommand cmd = new MySqlCommand(selectQueryStudent, conn);

                MySqlParameter factuurcodeParam = new MySqlParameter("factuur_code", MySqlDbType.Int32);
                factuurcodeParam.Value = factuur_code;
                cmd.Parameters.Add(factuurcodeParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    email = GetKlantEmailFromDataReader(dataReader);
                }

            }
            catch (Exception e)
            {
                Console.Write("Email niet opgehaald: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return email;
        }

        public bool GetKlantMembership(string usr)
        {
            try
            {
                conn.Open();

                string selectQuery = "SELECT * , SUM(F.totaalbedrag) AS GoldmembershipCheck FROM klant k " +
                    "LEFT OUTER JOIN goldmember G ON G.goldmembership = K.klantcode " +
                    "join factuur F on F.klant_code = k.klantcode " +
                    "WHERE K.gebruikersnaam =@username AND K.klantcode NOT IN(SELECT goldmembership " + 
                    "FROM goldmember) " + 
                    "HAVING GoldmembershipCheck >= 500;";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter usernameParam = new MySqlParameter("@username", MySqlDbType.VarChar);

                usernameParam.Value = usr;

                cmd.Parameters.Add(usernameParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if(dataReader.Read())
                    return true;
                else
                    return false;
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

        public List<Klant> GetKlant(string usr)
        {
            List<Klant> klanten = new List<Klant>();
            try
            {
                conn.Open();

                string selectQuery = "select * from klant where gebruikersnaam = @username";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter usernameParam = new MySqlParameter("@username", MySqlDbType.VarChar);

                usernameParam.Value = usr;

                cmd.Parameters.Add(usernameParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Klant klant = GetKlantFromDataReader(dataReader);
                    klanten.Add(klant);
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

            return klanten;
        }
        public bool InsertGoldMembership(int klantId)
        {
            try
            {
                conn.Open();

                string insertString = @"insert into goldmember (kortingspercentage, kalenderjaar, goldmembership) " +
                                                       " values (@kortingspercentage, NOW(), @goldmembership)";

                //commando en prepared statement
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter goldmemberParam = new MySqlParameter("@goldmembership", MySqlDbType.Int32);
                MySqlParameter kortingspercentageParam = new MySqlParameter("@kortingspercentage", MySqlDbType.Decimal);

                //set parameters naar view Waarden
                goldmemberParam.Value = klantId;
                kortingspercentageParam.Value = 5;

                //voeg parameters toe
                cmd.Parameters.Add(goldmemberParam);
                cmd.Parameters.Add(kortingspercentageParam);

                //prepare Execute Commit
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception e)
            {
                Console.Write("Inserten van gold membership mislukt" + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public void UpdateKlant(Klant klant)
        {
            try
            {
                conn.Open();

                string insertString = @"update Klant set naam=@klant_naam, adres=@klant_adres, woonplaats=@klant_woonplaats, telefoonnummer=@klant_telefoon, email=@klant_email where klantcode=@klant_id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter klantNaamParam = new MySqlParameter("@klant_naam", MySqlDbType.VarChar);
                MySqlParameter klantAdresParam = new MySqlParameter("@klant_adres", MySqlDbType.VarChar);
                MySqlParameter klantWoonplaatsParam = new MySqlParameter("@klant_woonplaats", MySqlDbType.VarChar);
                MySqlParameter klantTelefoonParam = new MySqlParameter("@klant_telefoon", MySqlDbType.VarChar);
                MySqlParameter klantEmailParam = new MySqlParameter("@klant_email", MySqlDbType.VarChar);
                MySqlParameter klantIdParam = new MySqlParameter("@klant_id", MySqlDbType.VarChar);

                klantNaamParam.Value = klant.Naam;
                klantAdresParam.Value = klant.Adres;
                klantWoonplaatsParam.Value = klant.Woonplaats;
                klantTelefoonParam.Value = klant.Telefoonnummer;
                klantEmailParam.Value = klant.Email;
                klantIdParam.Value = klant.Id;

                cmd.Parameters.Add(klantNaamParam);
                cmd.Parameters.Add(klantAdresParam);
                cmd.Parameters.Add(klantWoonplaatsParam);
                cmd.Parameters.Add(klantTelefoonParam);
                cmd.Parameters.Add(klantEmailParam);
                cmd.Parameters.Add(klantIdParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.Write("Updaten game niet gelukt: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }   
        }
        public List<KlantBestellingOverzicht> GetBestelling(string usr)
        {
            List<KlantBestellingOverzicht> bestellingen = new List<KlantBestellingOverzicht>();
            try
            {
                conn.Open();

                string selectQuery = "select factuur_code, product.naam, factuur.totaalbedrag, factuur.status " +
                                        "from product join factuur on productcode = product_code " +
                                        "join klant on factuur.klant_code = klant.klantcode " +
                                        "where klant.gebruikersnaam = @username";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter usernameParam = new MySqlParameter("@username", MySqlDbType.VarChar);

                usernameParam.Value = usr;

                cmd.Parameters.Add(usernameParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    KlantBestellingOverzicht bestelling = GetKlantBestellingFromDataReader(dataReader);
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

    }

}
