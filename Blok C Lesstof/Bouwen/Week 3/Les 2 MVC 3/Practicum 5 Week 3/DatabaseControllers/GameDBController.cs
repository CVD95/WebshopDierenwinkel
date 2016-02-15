using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkshopASPNETMVC_III_Start.Models;
using MySql.Data.MySqlClient;

namespace WorkshopASPNETMVC_III_Start.Databasecontrollers
{
    public class GameDBController : DatabaseController
    {
        public GameDBController() { }
            
        public void InsertGame(Game Game)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                string insertString = @"insert into Game (gamenaam, genre_id) values (@gamenaam, @genre_id)";
                
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter gamenaamParam = new MySqlParameter("@gamenaam", MySqlDbType.VarChar);
                MySqlParameter genreParam = new MySqlParameter("@genre_id", MySqlDbType.Int32);

                gamenaamParam.Value = Game.Naam;
                genreParam.Value = Game.Genre.ID;
                cmd.Parameters.Add(gamenaamParam);
                cmd.Parameters.Add(genreParam);
           
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                trans.Commit();
             
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Game niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public Game getGame(int gameID)
        {
            Game game = null;
           
            try
            {
                conn.Open();
                string selectGame = @"select * from game g, genre ge where g.genre_id = ge.genre_id and g.game_id = @gameid;";
                MySqlCommand cmd = new MySqlCommand(selectGame, conn);
                MySqlParameter gameidparameter = new MySqlParameter("@gameid", MySqlDbType.Int32);
                gameidparameter.Value = gameID;
                cmd.Parameters.Add(gameidparameter);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    Genre genre = GetGenreFromDataReader(dataReader);
                    game = GetGameFromDataReader(dataReader);
                    game.Genre = genre;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Game niet opgehaald");
                throw e;
            }
            finally
            {
                conn.Close();
            }
            
            return game;
        }

        public void UpdateGame(Game Game)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"update Game set gamenaam=@gamenaam, genre_id=@genre_id where game_id=@game_id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter gamenaamParam = new MySqlParameter("@gamenaam", MySqlDbType.VarChar);
                MySqlParameter genreParam = new MySqlParameter("@genre_id", MySqlDbType.Int32);
                MySqlParameter gameParam = new MySqlParameter("@game_id", MySqlDbType.Int32);

                gamenaamParam.Value = Game.Naam;
                genreParam.Value = Game.Genre.ID;
                gameParam.Value = Game.ID;

                cmd.Parameters.Add(gamenaamParam);
                cmd.Parameters.Add(genreParam);
                cmd.Parameters.Add(gameParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                trans.Commit();
              
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Updaten game niet gelukt: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }   
        }

        public void DeleteGame(int gameID)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"delete from game where game_id=@id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32);
                idParam.Value = gameID;

                cmd.Parameters.Add(idParam);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                trans.Commit();

            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Game niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void DeleteGame(Game Game)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"delete from game where game_id=@id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32);
                idParam.Value = Game.ID;

                cmd.Parameters.Add(idParam);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                
                trans.Commit();
                
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Game niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public void DeleteAllGames()
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"delete from game";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();

                trans.Commit();
               
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Games niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }


        public List<Game> GetGames()
        {
            List<Game> games = new List<Game>();
          

            try
            {
                conn.Open();
                string selectQuery = @"select game_id, ga.gamenaam, ge.genre_id, ge.genrenaam, verslavend 
                                        from game ga, genre ge 
                                        where ga.genre_id = ge.genre_id";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Genre genre = GetGenreFromDataReader(dataReader);
                    Game game = GetGameFromDataReader(dataReader);
                    game.Genre = genre;
                    
                    games.Add(game);
                }
               
            }
            catch(Exception e)
            {
                Console.Write("Ophalen van Games mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
            
            return games;
        }
    }
}
