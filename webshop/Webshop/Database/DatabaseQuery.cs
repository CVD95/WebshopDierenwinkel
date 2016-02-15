﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Npgsql;
using NpgsqlTypes;
using Webshop.Models;
using System.IO;
using Password;
using Webshop.Enums;
using Webshop.ViewModels;

namespace Webshop.Database
{
    public partial class DatabaseQuery
    {
        internal bool CreateUser(User user)
        { //create Query voor de users
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                PBKDF2Password password = new PBKDF2Password(user.Password);
                //Maak een speciaal geEncrypt Password aan.
                cmd.Connection = _connection;
                _transaction = cmd.Connection.BeginTransaction();

                cmd.CommandText = "INSERT INTO \"user\" (id, first_name, last_name, username, password_hash, password_salt, password_iterations, email_address, date_of_birth, role)"
                    + "VALUES(((SELECT COUNT(id) FROM \"user\")+1), @first_name, @last_name, @username, @password_hash, @password_salt, @password_iterations, @email_address, @date_of_birth, @role)";

                cmd.Parameters.AddWithValue("first_name", user.FirstName);
                cmd.Parameters.AddWithValue("last_name", user.LastName);
                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Parameters.AddWithValue("password_hash", password.Hash);
                cmd.Parameters.AddWithValue("password_salt", password.Salt);
                cmd.Parameters.AddWithValue("password_iterations", password.Iterations);
                cmd.Parameters.AddWithValue("email_address", user.Email);
                cmd.Parameters.AddWithValue("date_of_birth", user.DateOfBirth);
                cmd.Parameters.AddWithValue("role", (int)user.Role);

                //Parameters
                bool success = parseNonqueryResult(cmd.ExecuteNonQuery());
                if (success)
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                    setAddress(user);
                    return success; //Commit als het sucessvol is
                }
                _transaction.Rollback();
                _transaction.Dispose();
                return success; //Rollback en dispose als het niet lukt
            }
        }

        internal bool setAddress(User user)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = _connection;
                _transaction = cmd.Connection.BeginTransaction();

                cmd.CommandText = "INSERT INTO address (street, city, postalcode, number, suffix) "
                    + "VALUES(@street, @city, @postalcode, @number, @suffix); ";


                cmd.Parameters.AddWithValue("street", user.Address.Street);
                cmd.Parameters.AddWithValue("city", user.Address.City);
                cmd.Parameters.AddWithValue("postalcode", user.Address.PostalCode);
                cmd.Parameters.AddWithValue("number", user.Address.HouseNumber);
                cmd.Parameters.AddWithValue("suffix", user.Address.Suffix);

                //Parameters
                bool success = parseNonqueryResult(cmd.ExecuteNonQuery());
                if (success)
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                    setUserAddress(user);
                    return success; //Commit als het sucessvol is
                }
                _transaction.Rollback();
                _transaction.Dispose();
                return success; //Rollback en dispose als het niet lukt
            }
        }

        internal bool setUserAddress(User user)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = _connection;
                _transaction = cmd.Connection.BeginTransaction();

                cmd.CommandText = "INSERT INTO user_address (user_id, postalcode, number, suffix, type) "
                    + "VALUES(@user_id, @postalcode, @number, @suffix, @type);";

                LoginDataModel ldm = new LoginDataModel() { Username = user.Username, Password = user.Password };
                cmd.Parameters.AddWithValue("user_id", (long)GetUser(ldm).Id);
                cmd.Parameters.AddWithValue("postalcode", user.Address.PostalCode);
                cmd.Parameters.AddWithValue("number", user.Address.HouseNumber);
                cmd.Parameters.AddWithValue("suffix", user.Address.Suffix);
                cmd.Parameters.AddWithValue("type", (int)user.Address.Type);

                //Parameters
                bool success = parseNonqueryResult(cmd.ExecuteNonQuery());
                if (success)
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                    return success; //Commit als het sucessvol is
                }
                _transaction.Rollback();
                _transaction.Dispose();
                return success; //Rollback en dispose als het niet lukt
            }
        }

        internal User GetUser(LoginDataModel GetUser)

        { //Haal een user op uit de database
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = _connection;
                if (_transaction != null)
                {
                    cmd.Transaction = _transaction; //maak een transactie als hij er niet is
                }
                cmd.CommandText = "SELECT * FROM \"user\" WHERE \"username\" = @username";
                cmd.Parameters.AddWithValue("username", GetUser.Username);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) //lees een user
                {
                    User user = new User();
                    user.Id = (ulong)reader.GetInt32(reader.GetOrdinal("id"));
                    user.FirstName = reader.GetString(reader.GetOrdinal("first_name"));
                    user.LastName = reader.GetString(reader.GetOrdinal("last_name"));
                    user.Username = reader.GetString(reader.GetOrdinal("username"));
                    user.Email = reader.GetString(reader.GetOrdinal("email_address"));
                    user.DateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth"));
                    user.Role = (UserRole)Enum.ToObject(typeof(UserRole), reader.GetInt32(reader.GetOrdinal("role")));
                    reader.Close();
                    user.Address = GetAddress((ulong)reader.GetInt32(reader.GetOrdinal("id")));
                    return user;
                }
                reader.Close();
                return null;
            }
        }

        internal User GetUser(ulong userId)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = _connection;
                if (_transaction != null)
                {
                    cmd.Transaction = _transaction;
                }
                cmd.CommandText = "SELECT * FROM \"user\" where id= @userId";
                cmd.Parameters.AddWithValue("userId", userId);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    User user = new User
                    {
                        Id = (ulong)reader.GetInt32(reader.GetOrdinal("id")),
                        FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                        LastName = reader.GetString(reader.GetOrdinal("last_name")),
                        Username = reader.GetString(reader.GetOrdinal("username")),
                        Email = reader.GetString(reader.GetOrdinal("email_address")),
                        DateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                        Role = (UserRole)Enum.ToObject(typeof(UserRole), (int)reader.GetInt32(reader.GetOrdinal("role")))
                    };
                    reader.Close();
                    user.Address = GetAddress((ulong)reader.GetInt64(reader.GetOrdinal("id")));
                    return user;
                }
                reader.Close();
                return null; //Anders return een Null waarde
            }
        }


        internal Address GetAddress(ulong userId)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                cmd.CommandText = "SELECT * FROM user_address c join address a on a.postalcode=c.postalcode and a.number=c.number and a.suffix=c.suffix where user_id= @userId";
                cmd.Parameters.AddWithValue("userId", (long)userId);


                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Address address = new Address();

                    address.PostalCode = (string)reader.GetString(reader.GetOrdinal("postalcode"));
                    address.HouseNumber = (int)reader.GetInt64(reader.GetOrdinal("number"));
                    address.Suffix = (string)reader.GetString(reader.GetOrdinal("suffix"));
                    address.Street = (string)reader.GetString(reader.GetOrdinal("street"));
                    address.City = (string)reader.GetString(reader.GetOrdinal("city"));
                    address.Type = (AddressType)Enum.ToObject(typeof(AddressType), (int)reader.GetInt32(reader.GetOrdinal("type")));
                    reader.Close();
                    return address;
                }
                reader.Close();
                return null;
            }
        }

        internal Product GetProduct(ulong productId)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                cmd.CommandText = "SELECT product.name AS product_name, category.name AS category_name, category.id AS category_id, * FROM product AS product LEFT JOIN category AS category ON product.category_id = category.id WHERE product.id = @productId";
                cmd.Parameters.AddWithValue("productId", (long)productId);

                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) //lees een product
                {
                    Product product = new Product();
                    product.Id = productId;
                    product.ProductName = reader.GetString(reader.GetOrdinal("product_name"));
                    product.ProductDescription = reader.GetString(reader.GetOrdinal("description"));
                    product.Price = reader.GetDecimal(reader.GetOrdinal("price"));
                    product.Stock = (ulong)reader.GetInt64(reader.GetOrdinal("stock"));
                    product.BuyPrice = reader.GetDecimal(reader.GetOrdinal("buy_price"));

                    product.Category = new Category
                        {
                            Name = reader.GetString(reader.GetOrdinal("category_name")),
                            Id = (ulong)reader.GetInt32(reader.GetOrdinal("category_id"))
                        };
                    reader.Close();
                    return product;
                }
                reader.Close();
                return null; //Anders return je null
            }
        }





        internal Category GetCategory(ulong categoryId)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = _connection;
                cmd.CommandText = "SELECT * FROM category where id = @categoryId";
                cmd.Parameters.AddWithValue("categoryId", (long)categoryId);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Category category = new Category()
                    {
                        Id = (ulong)categoryId,
                        Name = reader.GetString(reader.GetOrdinal("name"))
                    };
                    return category;
                }
                reader.Close();
                return null;
            }
        }



        internal PBKDF2Password GetPassword(LoginDataModel customer)

        {   //Haal een password van een User op
            //The internal keyword is an access modifier for types and type members. Internal types or members are accessible only within files in the same assembly, 
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = _connection;
                cmd.CommandText = "SELECT password_hash, password_salt, password_iterations FROM \"user\" WHERE username = @username;";
                cmd.Parameters.AddWithValue("username", customer.Username.Trim());
                NpgsqlDataReader reader = cmd.ExecuteReader(); //Valideer het wachtwoord , Salt en Iteraties

                if (reader.Read())
                {

                    byte[] hash = (byte[])reader[reader.GetOrdinal("password_hash")];
                    byte[] salt = (byte[])reader[reader.GetOrdinal("password_salt")];
                    int iterations = reader.GetInt32(reader.GetOrdinal("password_iterations"));
                    return new PBKDF2Password(hash, salt, iterations);
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
        }

        //Orders kunen maar een paar statussen hebben
        internal bool CreateOrder(Product product, string customerName)
        {

            _transaction = this._connection.BeginTransaction(); //voor een order is er transactie nodig
            User customer = this.GetUser(new LoginDataModel { Username = customerName });
            //user moet een order maken
            using (NpgsqlCommand orderCmd = new NpgsqlCommand())
            {
                orderCmd.Connection = _connection;
                orderCmd.Transaction = _transaction;

                orderCmd.CommandText = "INSERT INTO \"order\"(id, date_time, \"user_id\", status) VALUES ((SELECT COUNT(id) FROM \"order\"), NOW(), @user_id, 0);";
                //Maak een order
                orderCmd.Parameters.AddWithValue("user_id", (int)customer.Id);

                if (parseNonqueryResult((int)orderCmd.ExecuteNonQuery()))
                {


                    using (NpgsqlCommand orderLineCmd = new NpgsqlCommand())
                    {
                        orderLineCmd.Connection = _connection;
                        orderLineCmd.Transaction = _transaction;

                        orderLineCmd.CommandText = "INSERT INTO \"orderline\" (product_id, order_id, amount) VALUES(@product_id, ((SELECT MAX(id) FROM \"order\")) , 1);";

                        orderLineCmd.Parameters.AddWithValue("product_id", (long)product.Id);
                        //Check of de Query kan worden uitgevoerd
                        bool sucess = parseNonqueryResult(orderLineCmd.ExecuteNonQuery());
                        if (sucess)
                        {
                            _transaction.Commit();
                            //als sucess dan mag de commit worden gedaan
                        }
                        else
                        {
                            _transaction.Rollback();
                            //anders verkom de Order
                        }
                        return true;
                        //Geef true om te bevestigen dat de order sucessvol is
                    }
                }
            }
            return false;
            //Anders is de order niet sucessvol
        }

        internal List<Product> GetProducts(string pattern = null)
        { //Krijg een lijst met producten
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                if (pattern == null)
                { //Als je niets invult dan moet je alle producten laten zien
                    cmd.CommandText = "SELECT * FROM product;";
                }
                else

                { //Anders mag je wat de user heeft ingetypt gebruiken als zoekparameters
                    cmd.CommandText = "SELECT * FROM product LEFT JOIN category ON product.category_id = category.id WHERE product.name LIKE '%' || @pattern || '%' OR description LIKE '%' || @pattern ||'%' OR category.name LIKE '%' || @pattern ||'%';";
                    cmd.Parameters.AddWithValue("pattern", pattern);
                }


                NpgsqlDataReader reader = cmd.ExecuteReader(); //intialiseren
                List<Product> products = new List<Product>();

                while (reader.Read())
                {
                    Product product = new Product();
                    product.Id = (ulong)reader.GetInt32(reader.GetOrdinal("id"));

                    product.BuyPrice = reader.GetDecimal(reader.GetOrdinal("buy_price"));
                    product.Price = reader.GetDecimal(reader.GetOrdinal("price"));

                    product.ProductName = reader.GetString(reader.GetOrdinal("name"));
                    product.ProductDescription = reader.GetString(reader.GetOrdinal("description"));

                    product.Category = this.GetCategory((ulong)reader.GetInt64(reader.GetOrdinal("category_id")));
                    product.Stock = (ulong)reader.GetInt64(reader.GetOrdinal("stock"));

                    //TODO: finish loading image properly
                    //product.image = byteArrayToImage((byte[])reader.GetValue(reader.GetOrdinal("image")));


                    products.Add(product); //Blijf producten toeveogen
                }

                reader.Close();
                return products; //return alle producten
            }
        }

        internal List<Category> GetCategories(string pattern = null)
        { //Krijg een lijst met categorien
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;

                    cmd.CommandText = "SELECT * FROM category;";

                NpgsqlDataReader reader = cmd.ExecuteReader(); //intialiseren
                List<Category> categories = new List<Category>();

                while (reader.Read())
                {
                    Category category = new Category();
                    category.Id = (ulong)reader.GetInt32(reader.GetOrdinal("id"));
                    category.Name = reader.GetString(reader.GetOrdinal("name"));

                    categories.Add(category); //Blijf categorien toeveogen
                }

                reader.Close();
                return categories; //return alle categorien
            }
        }

        internal List<User> GetUserDetails(string pattern = null)
        { //Krijg een lijst met categorien
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;

                cmd.CommandText = "SELECT * FROM user;";

                NpgsqlDataReader reader = cmd.ExecuteReader(); //intialiseren
                List<User> userDetails = new List<User>();

                while (reader.Read())
                { //Kan de ID code niet vinden  System.IndexOutOfRangeException: Field not found
                    User user = new User();
                    user.Id = (ulong)reader.GetInt64(reader.GetOrdinal("id"));
                    user.FirstName = reader.GetString(reader.GetOrdinal("first_name"));
                    user.FirstName = reader.GetString(reader.GetOrdinal("last_name"));
                    user.Email = reader.GetString(reader.GetOrdinal("email_adress"));
                    user.DateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth"));

                    userDetails.Add(user); //Blijf userdetails toeveogen
                }

                reader.Close();
                return userDetails; //return alle userdetails
            }
        }



        internal List<OrderLine> GetOrderlines()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                cmd.CommandText = "SELECT product_id, sum(amount) as NumberOfOrders FROM orderline group by product_id order by NumberOfOrders desc;";

                NpgsqlDataReader reader = cmd.ExecuteReader();
                List<OrderLine> orderlines = new List<OrderLine>();

                while (reader.Read())
                {
                    OrderLine orderline = new OrderLine();
                    orderline.Product = this.GetProduct((ulong)reader.GetInt64(reader.GetOrdinal("product_id")));
                    orderline.Amount = (ulong)reader.GetDecimal(reader.GetOrdinal("NumberOfOrders"));
                    orderlines.Add(orderline);
                }
                reader.Close();
                return orderlines;
            }
        }

        internal Image byteArrayToImage(byte[] byteArrayIn)

        { //Methose om een ByteArrat naar een Image te converten
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }



        internal bool CreateProduct(Product product)

        { //Maken van een product
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = _connection;
                _transaction = cmd.Connection.BeginTransaction();
                cmd.CommandText = "INSERT INTO product(name, description, price, buy_price, stock, category_id)"
                    + "VALUES(@name, @description, @price, @buy_price, @stock, @category_id);";

                cmd.Parameters.AddWithValue("name", product.ProductName);
                cmd.Parameters.AddWithValue("description", product.ProductDescription);
                cmd.Parameters.AddWithValue("price", product.Price);
                cmd.Parameters.AddWithValue("buy_price", product.BuyPrice);
                cmd.Parameters.AddWithValue("stock", (int)product.Stock);
                cmd.Parameters.AddWithValue("category_id", (long)product.Category.Id);
                //image
                //Parameters
                bool success = parseNonqueryResult(cmd.ExecuteNonQuery());
                if (success)
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                    return success; //Commit als het sucessvol is
                }
                _transaction.Rollback();
                _transaction.Dispose();
                return success;
            }
        }

        internal bool CreateCategory(Category category)
        { //Maken van een categorie
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = _connection;
                _transaction = cmd.Connection.BeginTransaction();
                cmd.CommandText = "INSERT INTO category(name) VALUES(@name);";

                cmd.Parameters.AddWithValue("name", category.Name);

                bool success = parseNonqueryResult(cmd.ExecuteNonQuery());
                if (success)
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                    return success; //Commit als het sucessvol is
                }
                _transaction.Rollback();
                _transaction.Dispose();
                return success;
            }
        }

        internal List<Order> GetOrders()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                cmd.CommandText = "SELECT * FROM \"order\" join orderlines";

                NpgsqlDataReader reader = cmd.ExecuteReader();
                List<Order> orders = new List<Order>();

                while (reader.Read())
                {
                    Order order = new Order();
                    order.Id = (ulong)reader.GetInt64(reader.GetOrdinal("id"));
                    order.DTime = (DateTime)reader.GetDateTime(reader.GetOrdinal("date_time"));
                    order.User = this.GetUser((ulong)reader.GetInt64(reader.GetOrdinal("user_id")));
                    order.OrderLines = this.getOrderlines(reader.GetOrdinal("id"));
                    orders.Add(order);
                }
                reader.Close();
                return orders;
                //voeg alle parameters toe an de query en Execute
            }
        }

        internal List<Category> GetCategories()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                cmd.CommandText = "SELECT * from category;";

                NpgsqlDataReader reader = cmd.ExecuteReader();
                List<Category> categories = new List<Category>();

                while (reader.Read())
                {
                    Category c = new Category();
                    c.Id = (ulong)reader.GetInt32(reader.GetOrdinal("id"));
                    c.Name = reader.GetString(reader.GetOrdinal("name"));
                    categories.Add(c);
                }
                reader.Close();
                return categories;
            }
        }


        internal List<OrderLine> getOrderlines(int orderId)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                cmd.CommandText = String.Format(@"SELECT * FROM orderline where order_id={0};", orderId);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                List<OrderLine> orderlines = new List<OrderLine>();

                while (reader.Read())
                {
                    OrderLine orderline = new OrderLine
                    {

                        Product = this.GetProduct((ulong)reader.GetOrdinal("product_id")),
                        Amount = (ulong)reader.GetInt64(reader.GetOrdinal("amount"))
                    };
                    orderlines.Add(orderline);
                }
                reader.Close();
                return orderlines;
            }
        }

        internal bool DeleteProduct(ulong productId)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                cmd.CommandText = "DELETE FROM product where id = @id;";
                cmd.Parameters.AddWithValue("id", (long)productId);
                bool succes = parseNonqueryResult(cmd.ExecuteNonQuery());
                return succes;
            }
        }

        internal bool UpdateProduct(Product product)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                _transaction = cmd.Connection.BeginTransaction();

                cmd.CommandText = "UPDATE product SET name=@name, description=@description, price=@price, buy_price=@buy_price, stock=@stock, category_id=@category_id where id = @id;";

                cmd.Parameters.AddWithValue("id", (long)product.Id);
                cmd.Parameters.AddWithValue("name", product.ProductName);
                cmd.Parameters.AddWithValue("description", product.ProductDescription);
                cmd.Parameters.AddWithValue("price", product.Price);
                cmd.Parameters.AddWithValue("buy_price", product.BuyPrice);
                cmd.Parameters.AddWithValue("stock", (int)product.Stock);
                cmd.Parameters.AddWithValue("category_id", (long)product.Category.Id);

                //Parameters
                bool success = parseNonqueryResult(cmd.ExecuteNonQuery());
                if (success)
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                }
                return success;
            }
        }

        internal bool DeleteCategory(ulong categoryId)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                cmd.CommandText = "DELETE FROM category where id = @id;";
                cmd.Parameters.AddWithValue("id", (long)categoryId);
                bool succes = parseNonqueryResult(cmd.ExecuteNonQuery());
                return succes;
            }
        }

        internal bool UpdateCategory(Category category)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                _transaction = cmd.Connection.BeginTransaction();

                cmd.CommandText = "UPDATE category SET name=@name where id = @id;";

                cmd.Parameters.AddWithValue("id", (long)category.Id);
                cmd.Parameters.AddWithValue("name", category.Name);
                
                //Parameters
                bool success = parseNonqueryResult(cmd.ExecuteNonQuery());
                if (success)
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                }
                return success;
            }
        }
    }
}