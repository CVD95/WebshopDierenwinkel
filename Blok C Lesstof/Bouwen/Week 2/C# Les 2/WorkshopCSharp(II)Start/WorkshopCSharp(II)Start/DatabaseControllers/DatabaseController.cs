using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace WorkshopCSharp_II_Start.DatabaseControllers
{
    public abstract class DatabaseController
    {
        protected MySqlConnection conn;

        public DatabaseController()
        {
            //Databassse = localhost, Database = Lanparty, user = root Pass = admin
            conn = new MySqlConnection("Server=localhost;Database=lanparty;Uid=root;Pwd=admin;");
        }
    }
}
