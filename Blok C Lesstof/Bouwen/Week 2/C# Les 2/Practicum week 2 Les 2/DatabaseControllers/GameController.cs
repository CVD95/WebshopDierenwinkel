using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

using WorkshopCSharp_II_Start.Models;

namespace WorkshopCSharp_II_Start.DatabaseControllers
{
    class GameController : DatabaseController
    {

        //CRUD functionaliteiten voor Game

        public GameController() { }

        //TODO Implementeer de onderstaande methoden tijdens het practicum
            
        public void InsertGame(Game Game)
        {
            MySqlTransaction trans = null;
            //transactie is null 

            try{
                //probeer deze code uit te voeren
            conn.Open();
            //open een verbinding met de database
            trans = conn.BeginTransaction();
            //begin de transactie.
            string insertString = @"INSERT INTO game (game_id, gamenaam, genre_id)
                                VALUES(@game_id, @gamenaam, @genre_id)";
            // query text
            MySqlCommand cmd = new MySqlCommand(insertString, conn);
            //maak een verbinding en zet de query in het command
            MySqlParameter gameID = new MySqlParameter("@game_id", MySqlDbType.Int32);
            // game_id is een int. Prepared statement.
            MySqlParameter gameNaam = new MySqlParameter("@gamenaam", MySqlDbType.String);
            // gamenaam is een string. Prepared statement
            MySqlParameter genreID = new MySqlParameter("@genre_id", MySqlDbType.Int32);
            // genreId is een inteer Prepared statement.

            gameID.Value = Game.ID;
            //Id van de game is Game.ID uit de Game.CS
            gameNaam.Value = Game.Naam;
            //naam van de game = Game Naam
            genreID.Value = Game.Genre.ID;
            //haal het Id van het genre op uit de game daaruir het id van het Genre

            cmd.Parameters.Add(gameID);
            //voeg het Game Id toe aan de parameters van het command naar de database.
            cmd.Parameters.Add(gameNaam);
            //voeg de gamenaam toe
            cmd.Parameters.Add(genreID);
            //voed het genre ID toe

            cmd.Prepare();
            //bereid de prepared statment voor;

            cmd.ExecuteNonQuery();
            //voer een INSERT UPDATE OF DELETE UIT

            trans.Commit();
            //als alles is goed gegaan voer de transactie uit.

            Console.Write("Genre is toegevoegd aan de database!");


        }
        catch(Exception error)
        {
            //als de try niet lukt dan gebeurt er dit
        trans.Rollback();
            //indien er een fout is dan wordt de transactie NIET uitgevoerd
        Console.Write("Game is niet toegevoegd" + error);
         }
            finally
                {
                    conn.Close();
                //sluit Altijd de verbinding met de database.
                }



        }

        public Game getGame()
        {

            return null;
        }

        public void UpdateGame(Game Game)
        {
        
        }

        public void DeleteGame(Game Game)
        {
 
        }
        public void DeleteAllGames()
        {
          
        }


        public List<Game> GetGames()
        {
            return null;
        }
    }
}
