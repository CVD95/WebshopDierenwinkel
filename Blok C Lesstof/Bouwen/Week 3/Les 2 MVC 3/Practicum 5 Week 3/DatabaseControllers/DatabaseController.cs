using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using WorkshopASPNETMVC_III_Start.Models;


namespace WorkshopASPNETMVC_III_Start.Databasecontrollers
{
    public abstract class DatabaseController
    {
        protected MySqlConnection conn;

        public DatabaseController()
        {
            conn = new MySqlConnection("Server=localhost;Database=lanparty;Uid=root;Pwd=admin");
        }

        protected Genre GetGenreFromDataReader(MySqlDataReader dataReader)
        {
            string genreNaam = dataReader.GetString("genrenaam");
            int genreId = dataReader.GetInt32("genre_id");
            bool verslavend = dataReader.GetBoolean("verslavend");
            Genre genre = new Genre { ID = genreId,Naam = genreNaam, Verslavend = verslavend};
           
            return genre;
        }

        protected Game GetGameFromDataReader(MySqlDataReader dataReader)
        {
            string gameNaam = dataReader.GetString("gamenaam");
            int gameId = dataReader.GetInt32("game_id");
            Game game = new Game { ID =gameId, Naam = gameNaam};
          

            return game;
        }

        protected Student GetStudentFromDataReader(MySqlDataReader dataReader)
        {
            int studentenID = dataReader.GetInt32("student_id");
            string naam = dataReader.GetString("studentnaam");
            DateTime geboorteDatum = dataReader.GetDateTime("geboortedatum");
            int studiePunten = dataReader.GetInt32("studiepunten");

            Student student = new Student { ID = studentenID, Naam = naam, GeboorteDatum = geboorteDatum, StudiePunten=studiePunten};
  

            return student;
        }

        protected Evenement GetEvenementFromDataReader(MySqlDataReader dataReader)
        {
            int evenementId = dataReader.GetInt32("evenement_id");
            Lokatie lokatie = (Lokatie)Enum.Parse(typeof(Lokatie), dataReader.GetString("lokatie"));
            DateTime datum = dataReader.GetDateTime("datum");
            string evenementNaam = dataReader.GetString("evenementnaam");

            Evenement evenement = new Evenement{ID = evenementId, Lokatie=lokatie, Datum = datum, Naam = evenementNaam};
       
            return evenement;
        }

        protected Inschrijving GetInschrijvingFromDataReader(MySqlDataReader dataReader)
        {
            bool eetMee = dataReader.GetBoolean("eetMee");
            bool betaald = dataReader.GetBoolean("betaald");

            Inschrijving inschrijving = new Inschrijving { EetMee = eetMee, Betaald = betaald};

            return inschrijving;
        }
    }
}
