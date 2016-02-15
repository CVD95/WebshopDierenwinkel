
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using IntoSport.Models;

namespace IntoSport.DatabaseControllers
{
    public class RegistratieDBController : DatabaseController 
    {
        public RegistratieDBController() { }
        /*
        // Gebruiker ID opzoeken
        public int GetKlantId(String email)
        {
            Int32 klantId = 18;
            try
            {
                Console.WriteLine("Call 1#");
                conn.Open();

                string selectQueryStudent = @"select max(klantcode) from klant where email = @email";
                MySqlCommand cmd = new MySqlCommand(selectQueryStudent, conn);

                MySqlParameter emailParam = new MySqlParameter("@email", MySqlDbType.VarChar);

                emailParam.Value = email;
                
                cmd.Parameters.Add(emailParam);

                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    klantId = dataReader.GetInt32(0);
                }
                return klantId;
            }
            catch (Exception e)
            {
                Console.Write("KlantId niet gevonden: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        */
        // Gebruiker toevoegen
        public void InsertKlant(RegistrerenModel registrerenViewModel)
            {
                try
                {
                    conn.Open();

                    // KolomNummering                           1               2               3               4               5               6               7               8
                    string insertString = @"insert into klant (gebruikersnaam,  wachtwoord,     naam,            adres,      woonplaats,     telefoonnummer,    email) 
                                                        values (@gebruikersnaam,@wachtwoord,    @naam,          @adres,     @woonplaats,    @telefoonnummer,    @email)";

                    MySqlCommand cmd = new MySqlCommand(insertString, conn);

                    MySqlParameter gebruikersnaamParam = new MySqlParameter("@gebruikersnaam", MySqlDbType.VarChar);
                    MySqlParameter wachtwoordParam = new MySqlParameter("@wachtwoord", MySqlDbType.VarChar);
                    MySqlParameter naamParam = new MySqlParameter("@naam", MySqlDbType.VarChar);
                    MySqlParameter adresParem = new MySqlParameter("@adres", MySqlDbType.VarChar);
                    MySqlParameter woonplaatsParem = new MySqlParameter("@woonplaats", MySqlDbType.VarChar);
                    MySqlParameter telefoonParam = new MySqlParameter("@telefoonnummer", MySqlDbType.VarChar); // Int32
                    MySqlParameter emailParam = new MySqlParameter("@email", MySqlDbType.VarChar);

                    gebruikersnaamParam.Value = registrerenViewModel.Gebruikersnaam;
                    wachtwoordParam.Value = registrerenViewModel.Wachtwoord;
                    naamParam.Value = registrerenViewModel.Naam;
                    adresParem.Value = registrerenViewModel.Adres;
                    woonplaatsParem.Value = registrerenViewModel.Woonplaats;
                    telefoonParam.Value = registrerenViewModel.Telefoonnummer;
                    emailParam.Value = registrerenViewModel.Email;

                    cmd.Parameters.Add(gebruikersnaamParam);
                    cmd.Parameters.Add(wachtwoordParam);
                    cmd.Parameters.Add(naamParam);
                    cmd.Parameters.Add(adresParem);
                    cmd.Parameters.Add(woonplaatsParem);
                    cmd.Parameters.Add(telefoonParam);
                    cmd.Parameters.Add(emailParam);

                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
             
                }
                catch (Exception e)
                {
                    Console.Write("Klant niet toegevoegd: " + e);
                    throw e;
                }
                finally
                {
                    conn.Close();
                }
            }
        public Boolean checkGebruikerDuplicaat(string gebruikersnaam)
        {
            try
            {
                conn.Open();

                String insertString = "select * from klant where gebruikersnaam = @gebruikersnaam";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);

                MySqlParameter gebruikersnaamParam = new MySqlParameter("@gebruikersnaam", MySqlDbType.VarChar);

                gebruikersnaamParam.Value = gebruikersnaam;

                cmd.Parameters.Add(gebruikersnaamParam);

                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (!dataReader.Read())
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
        // Voeg de klant zijn gebruikersnaam, wachtwoord en rechten toe aan de database.
        /*public void InsertKlantAccount(RegistrerenViewModel registrerenViewModel)
        {
            MySqlTransaction trans = null;
            try
            {

                conn.Open();
                trans = conn.BeginTransaction();
                // KolomNummering                           1                   2               3               4
                string insertString = @"insert into account (usernaam,    wachtwoord,     rechten,      klantcode) 
                                                    values (@gebruikersnaam,    @wachtwoord,    @rechten,     @klantcode)";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);

                // Account Gegevens
                MySqlParameter gebruikersnaamParem = new MySqlParameter("@gebruikersnaam", MySqlDbType.VarChar);
                MySqlParameter wachtwoordParem = new MySqlParameter("@wachtwoord", MySqlDbType.VarChar);
                MySqlParameter rechtenParem = new MySqlParameter("@rechten", MySqlDbType.Enum);
                MySqlParameter klantcodeParem = new MySqlParameter("@klantcode", MySqlDbType.Int32);

                // Klant ID zoeken
                String naam = registrerenViewModel.Naam;
                String telefoonnummer = registrerenViewModel.Telefoonnummer.ToString();
                String email = registrerenViewModel.Email;

                // Waardes toevoegen aan de parameters
                gebruikersnaamParem.Value = registrerenViewModel.Gebruikersnaam;
                wachtwoordParem.Value = registrerenViewModel.Wachtwoord;
                rechtenParem.Value = "klant";
                klantcodeParem.Value = GetKlantId(email);

                //int klantIdCheck = ;

                // Parameters toevoegen aan de MySqlCommand
                cmd.Parameters.Add(gebruikersnaamParem);
                cmd.Parameters.Add(wachtwoordParem);
                cmd.Parameters.Add(rechtenParem);
                cmd.Parameters.Add(klantcodeParem);

                // MySql Parameters samenvoegen en voorbereiden
                cmd.Prepare();

                // Execute Query
                cmd.ExecuteNonQuery();

                // Alle data moet goed zijn voordat het wordt toegevoegt aan de database
                trans.Commit();

            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Klant niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        // Omdat er 2 tabellen zijn moeten we zeker zijn dat ze beiden toegevoegt worden zonder problemen.
        public void InsertKlantEnAccount(RegistrerenViewModel registrerenViewModel)
        {
            // Beter om connectie hier te openen en door te geven aan de methodes
            // zodat we SQL transactie kunnen gebruiken voor een connectie over meerdere functies.
            // Dit zorgt ervoor dat ALLE data goed moet zijn voordat het in klant / account toevoegt.
            InsertKlant(registrerenViewModel);
            InsertKlantAccount(registrerenViewModel);
        }
         * */
    }
}
