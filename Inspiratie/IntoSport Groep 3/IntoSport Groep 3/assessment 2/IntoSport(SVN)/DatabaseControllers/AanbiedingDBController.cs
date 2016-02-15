using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntoSport.Models;
using MySql.Data.MySqlClient;

namespace IntoSport.DatabaseControllers
{
    public class AanbiedingDBController : DatabaseController
    {
        public List<Aanbieding> GetAangepastAanbiedingen()
        {
            List<Aanbieding> Aanbiedingen = new List<Aanbieding>();

            try
            {
                conn.Open();

                string selectQuery = "select * from aanbieding";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Aanbieding aanbieding = GetAanbiedingAangepastFromDataReader(dataReader);
                    Aanbiedingen.Add(aanbieding);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van Aanbiedingen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return Aanbiedingen;
        }

        public List<Aanbieding> GetAanbiedingen()
        {
            List<Aanbieding> Aanbiedingen = new List<Aanbieding>();

            try
            {
                conn.Open();

                string selectQuery = "select * from aanbieding";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Aanbieding aanbieding = GetAanbiedingFromDataReader(dataReader);
                    Aanbiedingen.Add(aanbieding);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van Aanbiedingen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return Aanbiedingen;
        }

        public Aanbieding GetAanbieding(int aanbiedingscode)
        {
            Aanbieding aanbieding = null;

            try
            {
                conn.Open();

                string selectQuery = "select * from aanbieding where aanbiedingscode=@aanbiedingscode";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter aanbiedingscodeParam = new MySqlParameter("aanbiedingscode", MySqlDbType.Int32);

                aanbiedingscodeParam.Value = aanbiedingscode;

                cmd.Parameters.Add(aanbiedingscodeParam);

                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    aanbieding = GetAanbiedingFromDataReader(dataReader);

                }


            }
            catch (Exception e)
            {

                Console.Write("Ophalen van aanbieding mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return aanbieding;
        }


        public void InsertAanbieding(Aanbieding aanbieding)
        {

            try
            {
                conn.Open();
                string insertString = @"insert into aanbieding (naam, kortingspercentage, geldig_tot) " +
                                        "values (@naam, @kortingspercentage, @geldig_tot)";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter naamParam = new MySqlParameter("@naam", MySqlDbType.VarChar);
                MySqlParameter kortingspercentageParam = new MySqlParameter("@kortingspercentage", MySqlDbType.Int32);
                MySqlParameter geldig_totParam = new MySqlParameter("@geldig_tot", MySqlDbType.DateTime);

                naamParam.Value = aanbieding.naam;
                kortingspercentageParam.Value = aanbieding.kortingspercentage;
                geldig_totParam.Value = aanbieding.geldig_tot;

                cmd.Parameters.Add(naamParam);
                cmd.Parameters.Add(kortingspercentageParam);
                cmd.Parameters.Add(geldig_totParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.Write("Aanbieding niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateAanbieding(Aanbieding aanbieding)
        {
            try
            {
                conn.Open();

                string insertString = @"update aanbieding set naam=@naam, kortingspercentage=@kortingspercentage, geldig_tot=@geldig_tot where aanbiedingscode=@aanbiedingscode";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter aanbiedingscodeParam = new MySqlParameter("@aanbiedingscode", MySqlDbType.Int32);
                MySqlParameter naamParam = new MySqlParameter("@naam", MySqlDbType.VarChar);
                MySqlParameter kortingspercentageParam = new MySqlParameter("@kortingspercentage", MySqlDbType.Int32);
                MySqlParameter geldig_totParam = new MySqlParameter("@geldig_tot", MySqlDbType.DateTime);

                naamParam.Value = aanbieding.naam;
                aanbiedingscodeParam.Value = aanbieding.aanbiedingscode;
                kortingspercentageParam.Value = aanbieding.kortingspercentage;
                geldig_totParam.Value = Convert.ToDateTime(aanbieding.geldig_tot).ToString("yyyy-MM-dd");

                cmd.Parameters.Add(naamParam);
                cmd.Parameters.Add(aanbiedingscodeParam);
                cmd.Parameters.Add(kortingspercentageParam);
                cmd.Parameters.Add(geldig_totParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Updaten aanbieding niet gelukt: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

        }

        public void VerwijderAanbieding(int aanbiedingscode)
        {
            try
            {
                conn.Open();

                string insertString = @"delete from aanbieding where aanbiedingscode=@aanbiedingscode";
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter aanbiedingscodeParam = new MySqlParameter("@aanbiedingscode", MySqlDbType.Int32);

                aanbiedingscodeParam.Value = aanbiedingscode;

                cmd.Parameters.Add(aanbiedingscodeParam);

                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("categorie niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
