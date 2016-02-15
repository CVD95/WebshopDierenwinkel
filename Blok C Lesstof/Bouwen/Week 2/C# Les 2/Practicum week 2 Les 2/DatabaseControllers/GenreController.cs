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
        {//geef een genre mee voor de insert.
            MySqlTransaction trans = null;
            //de transactie begint leeg,
            try
            {
                //open verbinding met de databasse
                conn.Open();
                trans = conn.BeginTransaction();
                //maak een transactie aan.
                string insertString = @"insert into genre (genrenaam, verslavend) 
                                               values (@genrenaam, @verslavend)";
                //maak de values voor de insert aan. Prepared statement.
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                //maak het command.
                MySqlParameter genrenaamParam = new MySqlParameter("@genrenaam", MySqlDbType.VarChar);
                //genrenaam is een vatchar in de database.
                MySqlParameter verslavendParam = new MySqlParameter("@verslavend", MySqlDbType.Bit);
                //verslavend is van het type bit (0,1)

                genrenaamParam.Value = genre.Naam;
                //genre.  Naam is de value van het SQL parameter.
                verslavendParam.Value = genre.Verslavend;
                // verslavend  0 , 1 is de value van de SQL parameter.

                cmd.Parameters.Add(genrenaamParam);
                //voeg de parameters toe aan het command.
                cmd.Parameters.Add(verslavendParam);
               

                cmd.Prepare();
                //prepare de statement
                cmd.ExecuteNonQuery();
                //voer een INSERT DELETE of UPDATE uit.
                trans.Commit();

            }
            catch (Exception e)
            {//als er iets misgaat.
                trans.Rollback(); //Transactie niet uitvoeren
                Console.Write("Genre niet toegevoegd: " + e); //print naar console foutmelding..
                throw e; //gooi een exceptions
            }
            finally
            {//voer alijd uit.
                conn.Close(); //verbreek de verbinding
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
        public Genre SelectGenre(int genreId)
        {
            Genre selectedGenre = new Genre();
            //inittialiseer Genre

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

                //maak een object aan van het type genre


                dataReader.Read();
                //Omdat het een resultaat is kan je geen while loop gebruiken.
                
                    //zolang er nog items in de table zitten
                    string genreNaam = dataReader.GetString("genrenaam");
                    //naam genre is de naam uit de table
                    int genreID = dataReader.GetInt32("genre_id");
                    bool verslavend = dataReader.GetBoolean("verslavend");

                    selectedGenre = new Genre { ID = genreID, Naam = genreNaam, Verslavend = verslavend };

               
            }
            catch(Exception error)
            {
                Console.WriteLine("Error" + error);
                //als er iets mis gaat print de error
            }
            finally
            {
                conn.Close();
                //sluit altijd de verbinding met de database
            }

            //als het programma klaar is dan wordt er een lijst gereturned
            return selectedGenre;
            //return de lijst met het genre erin

        }
    }
}


