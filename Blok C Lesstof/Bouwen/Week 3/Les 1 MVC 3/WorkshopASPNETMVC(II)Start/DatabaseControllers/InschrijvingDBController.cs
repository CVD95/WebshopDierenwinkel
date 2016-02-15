using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkshopASPNETMVC_II_Start.Models;
using MySql.Data.MySqlClient;

namespace WorkshopASPNETMVC_II_Start.Databasecontrollers
{
    public class InschrijvingDBController : DatabaseController
    {
        public InschrijvingDBController() { }

        public List<Inschrijving> GetInschrijvingenVanEvenement(int evenementID)
        {
            List<Inschrijving> inschrijvingen = new List<Inschrijving>();
            try
            {
                conn.Open();
              

                string selectQuery = @"select e.evenement_id, s.student_id, eetmee, betaald, lokatie, datum, e.evenementnaam, s.studentnaam 
	                                        , geboortedatum, studiepunten, g.gamenaam, g.game_id, ge.genre_id, ge.genrenaam, verslavend
                                       from inschrijving as i, evenement as e, student as s, game g, genre ge 
                                       where i.student_id = s.student_id and i.evenement_id = e.evenement_id and g.game_id = s.game_id and g.genre_id = ge.genre_id and e.evenement_id=@evenementid;";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter evenementParam = new MySqlParameter("@evenementid", MySqlDbType.Int32);
                evenementParam.Value = evenementID;
                cmd.Parameters.Add(evenementParam);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Evenement evenement = GetEvenementFromDataReader(dataReader);
                    Genre genre = GetGenreFromDataReader(dataReader);

                    Game game = GetGameFromDataReader(dataReader);
                    game.Genre = genre;

                    Student student = GetStudentFromDataReader(dataReader);
                    student.FavorieteSpel = game;

                    Inschrijving inschrijving = GetInschrijvingFromDataReader(dataReader);
                    inschrijving.Evenement = evenement;
                    inschrijving.Student = student;

                    inschrijvingen.Add(inschrijving);

                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van inschrijvingen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return inschrijvingen;
        }

        
        
        public List<Inschrijving> GetInschrijvingenVanStudent(int studentID)
        {
            List<Inschrijving> inschrijvingen = new List<Inschrijving>();
            try
            {
                conn.Open();
              
                string selectQuery = @"select e.evenement_id, s.student_id, eetmee, betaald, lokatie, datum, e.evenementnaam, s.studentnaam 
	                                        , geboortedatum, studiepunten, g.gamenaam, g.game_id, ge.genre_id, ge.genrenaam, verslavend
                                       from inschrijving as i, evenement as e, student as s, game g, genre ge 
                                       where i.student_id = s.student_id and i.evenement_id = e.evenement_id and g.game_id = s.game_id and g.genre_id = ge.genre_id and s.student_id=@studentid;";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter studentParam = new MySqlParameter("@studentid", MySqlDbType.Int32);
                studentParam.Value = studentID;
               
                cmd.Parameters.Add(studentParam);
              
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Evenement evenement = GetEvenementFromDataReader(dataReader);
                    Genre genre = GetGenreFromDataReader(dataReader);

                    Game game = GetGameFromDataReader(dataReader);
                    game.Genre = genre;

                    Student student = GetStudentFromDataReader(dataReader);
                    student.FavorieteSpel = game;

                    Inschrijving inschrijving = GetInschrijvingFromDataReader(dataReader);
                    inschrijving.Evenement = evenement;
                    inschrijving.Student = student;

                    inschrijvingen.Add(inschrijving);
                
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van inschrijvingen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return inschrijvingen;
        }

        
        public Inschrijving GetInschrijving(int studentID, int evenementID)
        {
            Inschrijving inschrijving = null;
          
            try
            {
                conn.Open();
              
                string selectQuery = @"select e.evenement_id, s.student_id, eetmee, betaald, lokatie, datum, e.evenementnaam, s.studentnaam 
	                                        , geboortedatum, studiepunten, g.gamenaam, g.game_id, ge.genre_id, ge.genrenaam, verslavend
                                       from inschrijving as i, evenement as e, student as s, game g, genre ge 
                                       where i.student_id = s.student_id and i.evenement_id = e.evenement_id and g.game_id = s.game_id and g.genre_id = ge.genre_id and s.student_id=@studentid and e.evenement_id=@evenementid;";
                
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter studentParam = new MySqlParameter("@studentid", MySqlDbType.Int32);
                MySqlParameter evenementParam = new MySqlParameter("@evenementid", MySqlDbType.Int32);
                cmd.Parameters.Add(studentParam);
                cmd.Parameters.Add(evenementParam);
            
                MySqlDataReader dataReader = cmd.ExecuteReader();

                dataReader.Read();

                Evenement evenement = GetEvenementFromDataReader(dataReader);
                Genre genre = GetGenreFromDataReader(dataReader);

                Game game = GetGameFromDataReader(dataReader);
                game.Genre = genre;

                Student student = GetStudentFromDataReader(dataReader);
                student.FavorieteSpel = game;

                inschrijving = GetInschrijvingFromDataReader(dataReader);
                inschrijving.Evenement = evenement;
                inschrijving.Student = student;
            }
            catch(Exception e)
            {
                Console.Write("Ophalen van inschrijving mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
            
            return inschrijving;
        }
        
        public void InsertInschrijving(Inschrijving inschrijving)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"insert into Inschrijving (student_id, evenement_id, betaald, eetmee) values (@studentid, @evenementid, @betaald, @eetmee)";
                
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter studentParam = new MySqlParameter("@studentid", MySqlDbType.Int32);
                MySqlParameter evenementParam = new MySqlParameter("@evenementid", MySqlDbType.Int32);
                MySqlParameter betaaldParam = new MySqlParameter("@betaald", MySqlDbType.Bit);
                MySqlParameter eetmeeParam = new MySqlParameter("@eetmee", MySqlDbType.Bit);
                
                studentParam.Value = inschrijving.Student.ID;
                evenementParam.Value = inschrijving.Evenement.ID;
                betaaldParam.Value = inschrijving.Betaald;
                eetmeeParam.Value = inschrijving.EetMee;

                cmd.Parameters.Add(studentParam);
                cmd.Parameters.Add(evenementParam);
                cmd.Parameters.Add(betaaldParam);
                cmd.Parameters.Add(eetmeeParam);
           
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                trans.Commit();
             
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Inschrijving niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        
        public void UpdateInschrijving(Inschrijving inschrijving)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"update Inschrijving set betaald=@betaald, eetmee=@eetmee where evenement_id=@evenementid and student_id=@studentid";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter betaaldParam = new MySqlParameter("@betaald", MySqlDbType.Bit);
                MySqlParameter eetmeeParam = new MySqlParameter("@eetmee", MySqlDbType.Bit);
                MySqlParameter evenementidParam = new MySqlParameter("@evenementid", MySqlDbType.Int32);
                MySqlParameter studentidParam = new MySqlParameter("@studentid", MySqlDbType.Int32);

                betaaldParam.Value = inschrijving.Betaald;
                eetmeeParam.Value = inschrijving.EetMee;
                evenementidParam.Value = inschrijving.Evenement.ID;
                studentidParam.Value = inschrijving.Student.ID;

                cmd.Parameters.Add(betaaldParam);
                cmd.Parameters.Add(eetmeeParam);
                cmd.Parameters.Add(evenementidParam);
                cmd.Parameters.Add(studentidParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                trans.Commit();
              
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Updaten inschrijving niet gelukt: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }   
        }

        public void DeleteInschrijving(int studentID, int evenementID)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"delete from inschrijving where student_id=@studentid and evenement_id=@evenementid";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter studentidParam = new MySqlParameter("@studentid", MySqlDbType.Int32);
                MySqlParameter evenementidParam = new MySqlParameter("@evenementid", MySqlDbType.Int32);

                studentidParam.Value = studentID;
                evenementidParam.Value = evenementID;

                cmd.Parameters.Add(studentidParam);
                cmd.Parameters.Add(evenementidParam);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                trans.Commit();

            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Inschrijving niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }


        public void DeleteInschrijving(Inschrijving inschrijving)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"delete from inschrijving where student_id=@studentid and evenement_id=@evenementid";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter studentidParam = new MySqlParameter("@studentid", MySqlDbType.Int32);
                MySqlParameter evenementidParam = new MySqlParameter("@evenementid", MySqlDbType.Int32);
                
                studentidParam.Value = inschrijving.Student.ID;
                evenementidParam.Value = inschrijving.Evenement.ID;

                cmd.Parameters.Add(studentidParam);
                cmd.Parameters.Add(evenementidParam);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                
                trans.Commit();
                
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Inschrijving niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void DeleteAllInschrijvingen()
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"delete from inschrijving";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();

                trans.Commit();
               
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Inschrijvingen niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }


        public List<Inschrijving> GetInschrijvingen()
        {
            List<Inschrijving> inschrijvingen = new List<Inschrijving>();
          
            try
            {
                conn.Open();
            
                string selectQuery = @"select e.evenement_id, s.student_id, eetmee, betaald, lokatie, datum, e.evenementnaam, s.studentnaam, geboortedatum, studiepunten, g.gamenaam, g.game_id, ge.genre_id, ge.genrenaam, verslavend
                                       from inschrijving as i, evenement as e, student as s, game g, genre ge 
                                       where i.student_id = s.student_id and i.evenement_id = e.evenement_id and g.game_id = s.game_id and g.genre_id = ge.genre_id;";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Evenement evenement = GetEvenementFromDataReader(dataReader);
                    Genre genre = GetGenreFromDataReader(dataReader);

                    Game game = GetGameFromDataReader(dataReader);
                    game.Genre = genre;

                    Student student = GetStudentFromDataReader(dataReader);
                    student.FavorieteSpel = game;

                    Inschrijving inschrijving = GetInschrijvingFromDataReader(dataReader);
                    inschrijving.Evenement = evenement;
                    inschrijving.Student = student;
                    inschrijvingen.Add(inschrijving);

                }
            }
            catch(Exception e)
            {
                Console.Write("Ophalen van inschrijvingen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
            
            return inschrijvingen;
        }
    }
}
