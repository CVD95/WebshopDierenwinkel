using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.IO;
using Webshop.ViewModels;

namespace Webshop.Database
{
    public partial class DatabaseQuery : IDisposable
    {
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction; //Database connectie en Transactie initialiser

        public DatabaseQuery()
        {
            _connection = new Connecter().Connection; //maak een Verbinding
        }

        private static bool parseNonqueryResult(int i)
        { //Helper Methode. Je krijgt normaal een 0 of een 1. Nu kan je er een boolean van maken Shorthand If statement -1 is een error die we willen voorkomen
            if (i == 0)
                return false;
            else if (i == 1)
                return true;
            else
                throw new ArgumentException("sql faillure, value must be 0 or 1, is this method really around an 'ExecuteNonquery()' function?");
        }

        private static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {  //Metohde om van een Image een Byte array te maken voor opslag in de database. 
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
             //check of de transactie en of de verbinding bestaat  voordat je hem disposed
            if (_transaction != null)
                _transaction.Dispose();            
        }
    }
}