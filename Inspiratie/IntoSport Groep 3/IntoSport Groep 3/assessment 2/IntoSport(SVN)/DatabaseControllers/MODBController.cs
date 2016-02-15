using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using IntoSport.Models;

namespace IntoSport.DatabaseControllers
{
    public class MODBController : DatabaseController
    {

        public List<Manager> GetOmzetOverzicht()
        {
            List<Manager> overzicht = new List<Manager>();

            try
            {
                conn.Open();


                //string selectQuery = "select  naam, verkoopprijs * voorraad as omzet, (verkoopprijs-inkoopprijs) * voorraad as winst from product";
                string selectQuery = "select naam, verkoopprijs * voorraad as omzet, (verkoopprijs-inkoopprijs) * voorraad as winst from product join factuur W on W.product_code = productcode  where datum >= DATE_SUB(NOW(), INTERVAL 1 YEAR) group by product_code";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Manager Moverzicht = GetOverzichtFromDataReader(dataReader);
                    overzicht.Add(Moverzicht);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van Overzicht mislukt " + e);

                throw e;
            }
            finally
            {
                conn.Close();
            }

            return overzicht;
        }

        public List<Manager> GetTopWinstOverzicht()
        {
            List<Manager> overzicht = new List<Manager>();

            try
            {
                conn.Open();
                string selectQuery = "select naam, verkoopprijs * count(W.product_code) as omzet, (verkoopprijs-inkoopprijs) * count(W.product_code) as winst from product join factuur W on W.product_code = productcode group by naam order by winst desc limit 10";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Manager Moverzicht = GetOverzichtFromDataReader(dataReader);
                    overzicht.Add(Moverzicht);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van Overzicht mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return overzicht;
        }

        public List<Manager> GetTopVerliesOverzicht()
        {
            List<Manager> overzicht = new List<Manager>();

            try
            {
                conn.Open();

                string selectQuery = "select naam, verkoopprijs * count(W.product_code) as omzet, (verkoopprijs-inkoopprijs) * count(W.product_code) as winst from product join factuur W on W.product_code = productcode group by naam order by winst limit 10";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Manager Moverzicht = GetOverzichtFromDataReader(dataReader);
                    overzicht.Add(Moverzicht);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van Overzicht mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return overzicht;
        }

        public List<Manager> GetOmzetOverzichtPerMaand(int maand, int jaar)
        {
            List<Manager> overzicht = new List<Manager>();

            try
            {
                conn.Open();

                string selectQuery = "select naam, Month(datum) as maand, Year(datum) as jaar,  verkoopprijs * count(Month(W.datum)) as omzet, (verkoopprijs - inkoopprijs) * count(Month(W.datum)) as winst from product join factuur W on W.product_code = productcode  where Month(datum) = @maand and Year(datum) = @jaar group by product_code";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter maandParam = new MySqlParameter("maand", MySqlDbType.Int32);
                MySqlParameter jaarParam = new MySqlParameter("jaar", MySqlDbType.Int32);

                maandParam.Value = maand;
                jaarParam.Value = jaar;
                cmd.Parameters.Add(maandParam);
                cmd.Parameters.Add(jaarParam);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Manager Moverzicht = GetOverzichtPerMaandFromDataReader(dataReader);
                    overzicht.Add(Moverzicht);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van Overzicht mislukt " + e);

                throw e;
            }
            finally
            {
                conn.Close();
            }

            return overzicht;
        }

    }
}
