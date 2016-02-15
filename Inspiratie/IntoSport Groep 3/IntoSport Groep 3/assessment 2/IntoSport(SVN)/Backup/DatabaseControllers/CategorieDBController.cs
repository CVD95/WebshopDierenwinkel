using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using IntoSport.Models;

namespace IntoSport.DatabaseControllers
{
    public class CategorieDBController : DatabaseController
    {
        public List<CategorieModel> GetCategorieen()
        {
            List<CategorieModel> Categorieen = new List<CategorieModel>();

            try
            {
                conn.Open();

                string selectQuery = "select * from categorie";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    CategorieModel categorie = getCategorieFromDataReader(dataReader);
                    Categorieen.Add(categorie);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van Categorieen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return Categorieen;
        }

        public void InsertCategorie(CategorieModel categorie, int producten, int sporten)
        {
            try
            {
                conn.Open();

                string insertString = @"insert into categorie (sport, product, producttype) values (@sport, @product, @type)";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter sportParam = new MySqlParameter("@sport", MySqlDbType.Int32);
                MySqlParameter productParam = new MySqlParameter("@product", MySqlDbType.Int32);
                MySqlParameter typeParam = new MySqlParameter("@type", MySqlDbType.VarChar);

                sportParam.Value = sporten;
                productParam.Value = producten;
                typeParam.Value = categorie.type;

                cmd.Parameters.Add(sportParam);
                cmd.Parameters.Add(productParam);
                cmd.Parameters.Add(typeParam);


                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Evenement niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void InsertCategorie(CategorieModel categorie, int producten)
        {
            try
            {
                conn.Open();

                string insertString = @"insert into categorie (sport, product, producttype) values (@sport, @product, @type)";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter sportParam = new MySqlParameter("@sport", MySqlDbType.Int32);
                MySqlParameter productParam = new MySqlParameter("@product", MySqlDbType.Int32);
                MySqlParameter typeParam = new MySqlParameter("@type", MySqlDbType.VarChar);

                sportParam.Value = categorie.sport;
                productParam.Value = producten;
                typeParam.Value = categorie.type;

                cmd.Parameters.Add(sportParam);
                cmd.Parameters.Add(productParam);
                cmd.Parameters.Add(typeParam);


                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Evenement niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }



        public void UpdateCategorie(CategorieModel categorie, int product, int sport)
        {
            try
            {
                conn.Open();

                string insertString = @"update categorie set sport=@sport, product=@product, producttype=@type where categorie_Id=@categorie_id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter sportParam = new MySqlParameter("@sport", MySqlDbType.Int32);
                MySqlParameter productParam = new MySqlParameter("@product", MySqlDbType.Int32);
                MySqlParameter typeParam = new MySqlParameter("@type", MySqlDbType.VarChar);
                MySqlParameter IDParam = new MySqlParameter("@categorie_id", MySqlDbType.Int32);


                sportParam.Value = sport;
                productParam.Value = product;
                typeParam.Value = categorie.type;
                IDParam.Value = categorie.id;


                cmd.Parameters.Add(sportParam);
                cmd.Parameters.Add(productParam);
                cmd.Parameters.Add(typeParam);
                cmd.Parameters.Add(IDParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Updaten categorie niet gelukt: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

        }


        public void DeleteCategorie(int categorie_id)
        {
            try
            {
                conn.Open();

                string insertString = @"delete from categorie where categorie_Id=@categorie_id";
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter sportParam = new MySqlParameter("@categorie_id", MySqlDbType.Int32);

                sportParam.Value = categorie_id;

                cmd.Parameters.Add(sportParam);

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



        public CategorieModel GetCategorie(int categorie_id)
        {
            CategorieModel categorie = null;

            try
            {
                conn.Open();

                string selectQuery = "select * from categorie where categorie_Id=@categorie_id";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter sportParam = new MySqlParameter("categorie_id", MySqlDbType.Int32);

                sportParam.Value = categorie_id;

                cmd.Parameters.Add(sportParam);

                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    categorie = getCategorieFromDataReader(dataReader);

                }


            }
            catch (Exception e)
            {

                Console.Write("Ophalen van categorie mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return categorie;
        }

        public List<Producten> GetSpecificProducten(int sport)
        {
            List<Producten> producten = new List<Producten>();

            try
            {
                conn.Open();

                string selectQuery = "select * from product P join categorie C on C.product = P.productcode where C.sport= @sport";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter sportParam = new MySqlParameter("sport", MySqlDbType.Int32);

                sportParam.Value = sport;
                cmd.Parameters.Add(sportParam);
                cmd.Prepare();
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

    }
}
