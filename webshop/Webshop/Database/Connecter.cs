using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using NpgsqlTypes;

namespace Webshop.Database
{
    public class Connecter : IDisposable
    {
        // Jouw database gegevens
        private string _server = "localhost";
        private string _database = "webshop";
        private string _username = "postgres";
        private string _password = "1234";

        public NpgsqlConnection Connection { get; private set; } //Private set mag alleen door de class zelf worden gebruikt
        public Connecter()
        {
            string connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};preload reader=true;", _server, _database, _username, _password);
            Connection = new NpgsqlConnection(connectionString); //Geef de je gegevens aan de connector mee
            Connection.Open(); //open connectie
            if (Connection.State == System.Data.ConnectionState.Open)
            {   //Als er verbinding kan worden gemaakt geef een message
                System.Diagnostics.Debug.Write("Success open postgreSQL connection."); 
            }
        }

        public void Dispose()
        { //stopt de connectie
            try
            {
                Connection.Close();
            }
            catch (Exception e)
            { //schrijf een error als jeniet kan disposen
                Console.WriteLine(e);
            }
        }
    }
}