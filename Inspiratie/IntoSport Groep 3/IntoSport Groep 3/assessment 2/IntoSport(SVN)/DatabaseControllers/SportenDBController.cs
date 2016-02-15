using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntoSport.Models;
using MySql.Data.MySqlClient;

namespace IntoSport.DatabaseControllers
{
    public class SportenDBController : DatabaseController
    {
        public List<Sport> GetSporten()
        {
            List<Sport> sporten = new List<Sport>();
            try
            {
                conn.Open();

                string selectQuery = "select * from sport";

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
                Console.Write("Ophalen van aanbiedingen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return sporten;
        }

        public Sport GetSport(int sportcode)
        {
            Sport sport = null;

            try
            {
                conn.Open();

                string selectQuery = "select * from sport where sportcode=@sportcode";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter sportcodeParam = new MySqlParameter("sportcode", MySqlDbType.Int32);
                sportcodeParam.Value = sportcode;
                cmd.Parameters.Add(sportcodeParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    sport = getSportenFromDataReader(dataReader);
                }

            }
            catch (Exception e)
            {

                Console.Write("Ophalen van Sport mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return sport;
        }

        public void InsertSport(Sport sport)
        {
            try
            { //open transactie en maak insert string en verbinding
                conn.Open();
              
                string insertString = @"insert into sport (naam, type) " +
                                                       " values (@naam, @type)";

                //commando en prepared statement
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter naamparameter = new MySqlParameter("@naam", MySqlDbType.String);
                MySqlParameter typeparameter = new MySqlParameter("@type", MySqlDbType.String);


                //set parameters naar view Waarden
                naamparameter.Value = sport.naam;
                typeparameter.Value = sport.type;

                //voeg parameters toe
                cmd.Parameters.Add(naamparameter);
                cmd.Parameters.Add(typeparameter);

                //prepare Execute Commit
                cmd.Prepare();
                cmd.ExecuteNonQuery();
               
            }
            catch (Exception e)
            {
                Console.Write("Sport niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateSport(Sport sport)
        {
            try
            {
                conn.Open();

                string insertString = @"UPDATE sport " +
                                        "SET naam=@sportnaam, type=@sporttype " +
                                         "WHERE sportcode=@sportId";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter sportcodeParameter = new MySqlParameter("@sportId", MySqlDbType.Int32);
                MySqlParameter sportnaamParameter = new MySqlParameter("@sportnaam", MySqlDbType.String);
                MySqlParameter sporttypeParameter = new MySqlParameter("@sporttype", MySqlDbType.String);


                sportcodeParameter.Value = sport.sportcode;
                sportnaamParameter.Value = sport.naam;
                sporttypeParameter.Value = sport.type;


                cmd.Parameters.Add(sportcodeParameter);
                cmd.Parameters.Add(sportnaamParameter);
                cmd.Parameters.Add(sporttypeParameter);



                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Updaten sport niet gelukt: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

        }

        public void VerwijderSport(int sportcode)
        {
            try
            {
                conn.Open();
                
                string insertString = @"delete from sport where sportcode=@id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter idCodeParam = new MySqlParameter("@id", MySqlDbType.Int32);

                idCodeParam.Value = sportcode;

                cmd.Parameters.Add(idCodeParam);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Sport niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
