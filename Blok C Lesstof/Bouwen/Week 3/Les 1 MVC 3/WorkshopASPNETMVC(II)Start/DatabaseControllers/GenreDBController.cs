using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkshopASPNETMVC_II_Start.Models;
using MySql.Data.MySqlClient;

namespace WorkshopASPNETMVC_II_Start.Databasecontrollers
{
    public class GenreDBController : DatabaseController
    {
        public GenreDBController() { }

        public Genre GetGenre(int genreID)
        {
            Genre genre = null;
            try
            {
                conn.Open();

                string selectQueryStudent = @"select * from genre where genre_id = @genreid";
                MySqlCommand cmd = new MySqlCommand(selectQueryStudent, conn);

                MySqlParameter genreidParam = new MySqlParameter("@genreid", MySqlDbType.Int32);
                genreidParam.Value = genreID;
                cmd.Parameters.Add(genreidParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    genre = GetGenreFromDataReader(dataReader);
                }

            }
            catch (Exception e)
            {
                Console.Write("Genre niet opgehaald: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return genre;
        }
    
        public void InsertGenre(Genre genre)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"insert into genre (genrenaam, verslavend) 
                                               values (@genrenaam, @verslavend)";
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter genrenaamParam = new MySqlParameter("@genrenaam", MySqlDbType.VarChar);
                MySqlParameter verslavendParam = new MySqlParameter("@verslavend", MySqlDbType.Bit);
                
                genrenaamParam.Value = genre.Naam;
                verslavendParam.Value = genre.Verslavend;

                cmd.Parameters.Add(genrenaamParam);
                cmd.Parameters.Add(verslavendParam);
           
                cmd.Prepare();

                cmd.ExecuteNonQuery();
                
                trans.Commit();
             
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Genre niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateGenre(Genre genre)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"update genre set genrenaam=@genrenaam, verslavend=@verslavend where genre_id=@id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter genrenaamParam = new MySqlParameter("@genrenaam", MySqlDbType.VarChar);
                MySqlParameter verslavendParam = new MySqlParameter("@verslavend", MySqlDbType.Bit);
                MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32);

                genrenaamParam.Value = genre.Naam;
                verslavendParam.Value = genre.Verslavend;
                idParam.Value = genre.ID;

                cmd.Parameters.Add(genrenaamParam);
                cmd.Parameters.Add(verslavendParam);
                cmd.Parameters.Add(idParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                trans.Commit();
              
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Genre niet upgedate: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }   
        }

        public void DeleteGenre(int genreId)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"delete from genre where genre_id=@id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32);
                idParam.Value = genreId;

                cmd.Parameters.Add(idParam);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                trans.Commit();

            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Genre niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void DeleteGenre(Genre genre)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"delete from genre where genre_id=@id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32);
                idParam.Value = genre.ID;

                cmd.Parameters.Add(idParam);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                
                trans.Commit();
                
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Genre niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public void DeleteAllGenres()
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"delete from genre";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();

                trans.Commit();
               
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Genre niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }


        public List<Genre> GetGenres()
        {
            List<Genre> genres = new List<Genre>();
            try
            {
                conn.Open();
                
                string selectQuery = "select * from genre";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Genre genre = GetGenreFromDataReader(dataReader);
                    genres.Add(genre);
                }
            }
            catch(Exception e)
            {
                Console.Write("Ophalen van genres mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
            
            return genres;
        }
    }
}
