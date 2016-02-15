using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkshopASPNETMVC_II_Start.Models;


using MySql.Data.MySqlClient;

namespace WorkshopASPNETMVC_II_Start.Databasecontrollers
{
    public class StudentDBController : DatabaseController
    {
        private InschrijvingDBController inschrijvingController = new InschrijvingDBController();

        public StudentDBController() { }

        public Student GetStudent(int studentID) 
        {
            Student student = null;
            try
            {
                conn.Open();
            
                string selectQueryStudent = @"select student_id, s.studentnaam, geboortedatum, studiepunten, g.game_id , g.gamenaam, ge.genre_id , ge.genrenaam, verslavend 
                                       from student s, game g, genre ge 
                                       where s.game_id = g.game_id and ge.genre_id = g.genre_id and student_id = @studentid;";

                MySqlCommand cmd = new MySqlCommand(selectQueryStudent, conn);
                
                MySqlParameter studentidParam = new MySqlParameter("@studentid", MySqlDbType.Int32);
                studentidParam.Value = studentID;
                cmd.Parameters.Add(studentidParam);
                cmd.Prepare();
                
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    Genre genre = GetGenreFromDataReader(dataReader);

                    Game game = GetGameFromDataReader(dataReader);
                    game.Genre = genre;

                    student = GetStudentFromDataReader(dataReader);
                    student.FavorieteSpel = game;

                    List<Inschrijving> inschrijvingen = inschrijvingController.GetInschrijvingenVanStudent(student.ID);
                    student.Inschrijvingen = inschrijvingen;
                }

              
            }
            catch (Exception e)
            {
                
                Console.Write("Student niet opgehaald: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return student;
        
        }
        
        public void InsertStudent(Student Student)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = @"insert into student (studentnaam, geboortedatum, studiepunten, game_id) values (@studentnaam, @geboortedatum, @studiepunten, @game_id)";
                
                MySqlCommand cmd = new MySqlCommand(insertString, conn);

                MySqlParameter studentnaamParam = new MySqlParameter("@studentnaam", MySqlDbType.VarChar);
                MySqlParameter geboortedatumParam = new MySqlParameter("@geboortedatum", MySqlDbType.DateTime);
                MySqlParameter studiepuntenParam = new MySqlParameter("@studiepunten", MySqlDbType.Int32);
                MySqlParameter gameParam = new MySqlParameter("@game_id", MySqlDbType.Int32);

                studentnaamParam.Value = Student.Naam;
                geboortedatumParam.Value = Student.GeboorteDatum;
                studiepuntenParam.Value = Student.StudiePunten;
                gameParam.Value = Student.FavorieteSpel.ID;

                cmd.Parameters.Add(studentnaamParam);
                cmd.Parameters.Add(geboortedatumParam);
                cmd.Parameters.Add(studiepuntenParam);
                cmd.Parameters.Add(gameParam);
           
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                trans.Commit();
             
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Student niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateStudent(Student Student)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string updateString = @"update student set studentnaam=@studentnaam, geboortedatum=@geboortedatum, studiepunten=@studiepunten, game_id=@gameid where student_id=@studentid";

                MySqlCommand cmd = new MySqlCommand(updateString, conn);
                MySqlParameter studentnaamParam = new MySqlParameter("@studentnaam", MySqlDbType.VarChar);
                MySqlParameter geboortedatumParam = new MySqlParameter("@geboortedatum", MySqlDbType.DateTime);
                MySqlParameter studiepuntenParam = new MySqlParameter("@studiepunten", MySqlDbType.Int32);
                MySqlParameter gameidParam = new MySqlParameter("@gameid", MySqlDbType.Int32);
                MySqlParameter studentidParam = new MySqlParameter("@studentid", MySqlDbType.Int32);


                studentnaamParam.Value = Student.Naam;
                geboortedatumParam.Value = Student.GeboorteDatum;
                studiepuntenParam.Value = Student.StudiePunten;
                gameidParam.Value = Student.FavorieteSpel.ID;
                studentidParam.Value = Student.ID;


                cmd.Parameters.Add(studentnaamParam);
                cmd.Parameters.Add(geboortedatumParam);
                cmd.Parameters.Add(gameidParam);
                cmd.Parameters.Add(studiepuntenParam);
                cmd.Parameters.Add(studentidParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                trans.Commit();
              
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Updaten student mislukt: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }   
        }

        public void DeleteStudent(int studentID)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string deleteString = @"delete from student where student_id=@id";

                MySqlCommand cmd = new MySqlCommand(deleteString, conn);
                MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32);
                idParam.Value = studentID;

                cmd.Parameters.Add(idParam);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                trans.Commit();

            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Student niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        

        public void DeleteStudent(Student Student)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string deleteString = @"delete from student where student_id=@id";

                MySqlCommand cmd = new MySqlCommand(deleteString, conn);
                MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32);
                idParam.Value = Student.ID;

                cmd.Parameters.Add(idParam);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                
                trans.Commit();
                
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Student niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public void DeleteAllStudents()
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string deleteString = @"delete from Student";

                MySqlCommand cmd = new MySqlCommand(deleteString, conn);
                cmd.ExecuteNonQuery();

                trans.Commit();
               
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.Write("Student niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }


        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
         
            try
            {
                conn.Open();
              
                string selectQuery = @"select student_id, s.studentnaam, geboortedatum, studiepunten, g.game_id , g.gamenaam, ge.genre_id , ge.genrenaam, verslavend 
                                       from student s, game g, genre ge 
                                       where s.game_id = g.game_id and ge.genre_id = g.genre_id;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                   
                    Genre genre = GetGenreFromDataReader(dataReader);
                    Game game = GetGameFromDataReader(dataReader);
                    game.Genre = genre;
                    Student student = GetStudentFromDataReader(dataReader);
                    student.FavorieteSpel = game;
                    
                    students.Add(student);
                }
            
            }
            catch(Exception e)
            {
                Console.Write("Ophalen van Studenten mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
            
            return students;
        }
    }
}
