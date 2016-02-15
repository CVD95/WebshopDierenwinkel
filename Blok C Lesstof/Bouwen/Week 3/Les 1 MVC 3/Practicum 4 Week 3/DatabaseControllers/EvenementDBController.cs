using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkshopASPNETMVC_II_Start.Models;
using MySql.Data.MySqlClient;

namespace WorkshopASPNETMVC_II_Start.Databasecontrollers
{
    public class EvenementDBController : DatabaseController
    {
        public EvenementDBController() { }

        private InschrijvingDBController inschrijvingController = new InschrijvingDBController();

        public Evenement GetEvenement(int evenementID)
        {
            Evenement evenement = null;
          
            try
            {
                conn.Open();
               
                string selectQuery = "select * from evenement where evenement_id=@evenementid";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter idParam = new MySqlParameter("evenementid", MySqlDbType.Int32);
                idParam.Value = evenementID;
                cmd.Parameters.Add(idParam); 
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    evenement = GetEvenementFromDataReader(dataReader);
                    //Ophalen van bijbehorende inschrijvingen
                    List<Inschrijving> inschrijvingen =  inschrijvingController.GetInschrijvingenVanEvenement(evenement.ID);
                    evenement.Inschrijvingen = inschrijvingen;
                }
            }
            catch (Exception e)
            {
              
                Console.Write("Ophalen van Evenement mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return evenement;
        }
        
        public void InsertEvenement(Evenement Evenement)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"insert into evenement (evenementnaam, datum, lokatie) values (@evenementnaam, @datum, @lokatie)";
                
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter evenementnaamParam = new MySqlParameter("@evenementnaam", MySqlDbType.VarChar);
                MySqlParameter datumParam = new MySqlParameter("@datum", MySqlDbType.DateTime);
                MySqlParameter lokatieParam = new MySqlParameter("@lokatie", MySqlDbType.Enum);

                evenementnaamParam.Value = Evenement.Naam;
                datumParam.Value = Evenement.Datum;
                lokatieParam.Value = Evenement.Lokatie;

                cmd.Parameters.Add(evenementnaamParam);
                cmd.Parameters.Add(datumParam);
                cmd.Parameters.Add(lokatieParam);

           
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Evenement niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateEvenement(Evenement evenement)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"update evenement set evenementnaam=@evenementnaam, datum=@datum, lokatie=@lokatie where evenement_id=@evenement_id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter evenementnaamParam = new MySqlParameter("@evenementnaam", MySqlDbType.VarChar);
                MySqlParameter datumParam = new MySqlParameter("@datum", MySqlDbType.DateTime);
                MySqlParameter lokatieParam = new MySqlParameter("@lokatie", MySqlDbType.Enum);
                MySqlParameter evenementParam = new MySqlParameter("@evenement_id", MySqlDbType.Int32);

                evenementnaamParam.Value = evenement.Naam;
                datumParam.Value = evenement.Datum;
                lokatieParam.Value = evenement.Lokatie;
                evenementParam.Value = evenement.ID;

                cmd.Parameters.Add(evenementnaamParam);
                cmd.Parameters.Add(datumParam);
                cmd.Parameters.Add(lokatieParam);
                cmd.Parameters.Add(evenementParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Updaten evenement niet gelukt: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        
        }

        public void DeleteEvenement(int evenementId)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"delete from Evenement where Evenement_id=@id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32);

                idParam.Value = evenementId;

                cmd.Parameters.Add(idParam);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Evenement niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }


        public void DeleteEvenement(Evenement evenement)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"delete from Evenement where Evenement_id=@id";
                
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32);
                
                idParam.Value = evenement.ID;

                cmd.Parameters.Add(idParam);
                cmd.Prepare();
                
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Evenement niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public void DeleteAllEvenementen()
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"delete from evenement";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();

                trans.Commit();
               
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Evenementen niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }


        public List<Evenement> GetEvenementen()
        {
            List<Evenement> evenementen = new List<Evenement>();
            MySqlTransaction trans = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                string selectQuery = "select * from Evenement";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn, trans);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                   Evenement Evenement = GetEvenementFromDataReader(dataReader);
                   evenementen.Add(Evenement);
                }
            }
            catch(Exception e)
            {
                Console.Write("Ophalen van Evenementen mislukt " + e);
                trans.Rollback();
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return evenementen;
        }
    }
}
