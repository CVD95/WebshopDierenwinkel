using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using WorkshopCSharp_II_Start.Models;

namespace WorkshopCSharp_II_Start.DatabaseControllers
{
    class GenreController : DatabaseController
    {
        //CRUD functionaliteiten voor Genre

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
                    string genreNaam = dataReader.GetString("genrenaam");
                    int genreId = dataReader.GetInt32("genre_id");
                    bool verslavend = dataReader.GetBoolean("verslavend");
                    Genre genre = new Genre {ID = genreId, Naam = genreNaam, Verslavend = verslavend};
                   
                    genres.Add(genre);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van genres mislukt " + e);
            }
            finally
            {
                conn.Close();
            }

            return genres;
        }

        public void DeleteAllGenres()
        {
            MySqlTransaction trans = null;
            //initialiseer de transactie
            try
            {
                //porbeer de volgende code
                conn.Open();
                //open de verbinding met de database
                trans = conn.BeginTransaction();
                ///maak de transactie aan
                string insertString = @"DELETE FROM genre";
                //Maak de string voor de Delete

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                //maak een commando waar de string in de connectie wordt gezet
                cmd.ExecuteNonQuery();
                //Voer een UPDATE DELETE OF INSERT uit

                trans.Commit();
                //Voer de transactie uit

                Console.Write("Alle genres succesvol verwijderd");
                //als de transactie succesvol is verlopen dan krijg je een bevestiging

            }
            catch (Exception e)
            {
                //als er iets misgaat dan wordt de catch uitgevoerd.
                trans.Rollback();
                //voer de transactie niet uit.
                Console.Write("Genres niet verwijderd: " + e);
                //vertel de console wat de error is
            }
            finally
            {
                conn.Close();
                //Finally wordt alijd uitgevoerd
                //sluit de verbinding.
            }
        }

        //TODO implementeren tijdens workshop
        public void UpdateGenre(Genre genre)
        {

        }

        //TODO implementeren tijdens workshop
        public void DeleteGenre(Genre genre)
        {

        }

        //TODO implementeren tijdens workshop
        public List<Genre> SelectGenre(int genreId)
        {
            List<Genre> genre = new List<Genre>();
            //initialiseer de transactie.
            try
            {
                conn.Open();
                //open de verbinding
                string insertString = @"SELECT * FROM genre WHERE genre_id =" + genreId;
                //maak de string voor de insert

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                //maak een nieuw command aan, Connectie en string meegeven

                MySqlDataReader dataReader = cmd.ExecuteReader();
                //maak een datareader aan 

                while(dataReader.Read())
                {
                    //zolang er nog items in de table zitten
                    string genreNaam = dataReader.GetString("genrenaam");
                    //naam genre is de naam uit de table
                    int genreID = dataReader.GetInt32("genre_id");
                    bool verslavend = dataReader.GetBoolean("verslavend");

                    Genre selectedGenre = new Genre {ID = genreID, Naam = genreNaam, Verslavend = verslavend};


                    genre.Add(selectedGenre);
                }
                
            }
            catch(Exception error)
            {
                Console.Write("Error" + error);
            }
            finally
            {
                conn.Close();
            }

            return genre;

        }
    }
}


