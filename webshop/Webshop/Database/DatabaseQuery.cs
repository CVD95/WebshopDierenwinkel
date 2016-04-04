using System;
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

                cmd.CommandText = "INSERT INTO address (street, city, postalcode, \"number\", suffix) "
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
                cmd.Parameters.AddWithValue("userId", (long)userId);
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
                    user.Address = GetAddress((ulong)reader.GetInt32(reader.GetOrdinal("id")));
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
                cmd.CommandText = "SELECT * FROM user_address c join address a on a.postalcode=c.postalcode and a.\"number\"=c.\"number\" and a.suffix=c.suffix where user_id= @userId";
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
                cmd.CommandText = "SELECT * FROM product WHERE product.id=@productId;";            
                cmd.Parameters.AddWithValue("productId", (long)productId);

                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) //lees een product
                {
                    Product product = new Product();
                    product.Id = (ulong)reader.GetInt32(reader.GetOrdinal("id"));
                    product.BuyPrice = reader.GetDecimal(reader.GetOrdinal("buy_price"));
                    product.Price = reader.GetDecimal(reader.GetOrdinal("price"));
                    product.ProductName = reader.GetString(reader.GetOrdinal("name"));
                    product.ProductDescription = reader.GetString(reader.GetOrdinal("description"));
                    product.Category = this.GetCategory((ulong)reader.GetInt64(reader.GetOrdinal("category_id")));
                    product.Stock = (ulong)reader.GetInt64(reader.GetOrdinal("stock"));
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

        internal List<Order> GetOrdersByUser(User user)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                //kan ook join gebruiken o join orderline ol on ol.order_id = o.id 
                cmd.CommandText = "SELECT * FROM \"order\" where user_id=@user_id";
                cmd.Parameters.AddWithValue("user_id", (long)user.Id);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                List<Order> orders = new List<Order>();

                while (reader.Read())
                {
                    Order order = new Order();
                    order.Id = (ulong)reader.GetInt32(reader.GetOrdinal("id"));
                    order.DTime = (DateTime)reader.GetDateTime(reader.GetOrdinal("date_time"));
                    order.User = this.GetUser((ulong)reader.GetInt64(reader.GetOrdinal("user_id")));
                    order.OrderLines = this.getOrderlines(order.Id);
                    order.Status = (OrderStatus)Enum.ToObject(typeof(OrderStatus), reader.GetInt64(reader.GetOrdinal("order_status")));
                    orders.Add(order);
                }
                reader.Close();
                return orders;
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
        internal bool CreateOrder(Order order)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = _connection;
                _transaction = cmd.Connection.BeginTransaction();
                cmd.CommandText = "INSERT INTO \"order\"(date_time, user_id, order_status)"
                    + "VALUES(@date_time, @user_id, @order_status);";

                cmd.Parameters.AddWithValue("date_time", order.DTime);
                cmd.Parameters.AddWithValue("user_id", (long)order.User.Id);
                cmd.Parameters.AddWithValue("order_status", (int)order.Status);

                bool success = parseNonqueryResult(cmd.ExecuteNonQuery());
                if (success)
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                    foreach (OrderLine ol in order.OrderLines)
                    {
                        CreateOrderline(ol, GetOrderId(order));
                    }
                    return success; //Commit als het sucessvol is
                }
                _transaction.Rollback();
                _transaction.Dispose();
                return success;
            }
        }

        internal bool CreateOrderline(OrderLine ol, ulong orderId)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = _connection;
                _transaction = cmd.Connection.BeginTransaction();
                cmd.CommandText = "INSERT INTO orderline(product_id, order_id, amount)"
                    + "VALUES(@product_id, @order_id, @amount);";

                cmd.Parameters.AddWithValue("product_id", (long)ol.Product.Id);
                cmd.Parameters.AddWithValue("order_id", (long)orderId);
                cmd.Parameters.AddWithValue("amount", (long)ol.Amount);

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

        internal bool UpdateOrderStatus(Order order)
        { //Maken van een product
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                _transaction = cmd.Connection.BeginTransaction();
                cmd.CommandText = "UPDATE \"order\" SET order_status=@order_status where id = @id;";

                cmd.Parameters.AddWithValue("order_status", (int)order.Status);
                cmd.Parameters.AddWithValue("id", (long)order.Id);

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

        internal List<PaymentOption> GetPaymentOptions()
        { //Krijg een lijst met categorien
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;

                cmd.CommandText = "SELECT * FROM pay_options;";

                NpgsqlDataReader reader = cmd.ExecuteReader(); //intialiseren
                List<PaymentOption> paymentOptions = new List<PaymentOption>();

                while (reader.Read())
                {
                    PaymentOption paymentOption = new PaymentOption();
                    paymentOption.Id = (ulong)reader.GetInt32(reader.GetOrdinal("id"));
                    paymentOption.Name = reader.GetString(reader.GetOrdinal("name"));
                    paymentOptions.Add(paymentOption); //Blijf categorien toeveogen
                }

                reader.Close();
                return paymentOptions; //return alle categorien
            }
        }

        internal List<User> GetUsers()
        { //Krijg een lijst met categorien
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;

                cmd.CommandText = "select distinct * from \"user\" u join user_address ua on ua.user_id = u.id join address a on ua.postalcode = a.postalcode and ua.\"number\" = a.\"number\" and ua.suffix = a.suffix";

                NpgsqlDataReader reader = cmd.ExecuteReader(); //intialiseren
                List<User> users = new List<User>();

                while (reader.Read())
                { //Kan de ID code niet vinden  System.IndexOutOfRangeException: Field not found
                    User user = new User();
                    user.Id = (ulong)reader.GetInt32(reader.GetOrdinal("id"));
                    user.FirstName = reader.GetString(reader.GetOrdinal("first_name"));
                    user.LastName = reader.GetString(reader.GetOrdinal("last_name"));
                    user.Username = reader.GetString(reader.GetOrdinal("username"));
                    user.Email = reader.GetString(reader.GetOrdinal("email_address"));
                    user.DateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth"));
                    user.Role = (UserRole)Enum.ToObject(typeof(UserRole), reader.GetInt32(reader.GetOrdinal("role")));
                    /*
                    user.Address.PostalCode = (string)reader.GetString(reader.GetOrdinal("postalcode"));
                    user.Address.HouseNumber = (int)reader.GetInt64(reader.GetOrdinal("number"));
                    user.Address.Suffix = (string)reader.GetString(reader.GetOrdinal("suffix"));
                    user.Address.Street = (string)reader.GetString(reader.GetOrdinal("street"));
                    user.Address.City = (string)reader.GetString(reader.GetOrdinal("city"));
                    user.Address.Type = (AddressType)Enum.ToObject(typeof(AddressType), (int)reader.GetInt32(reader.GetOrdinal("type")));
                    */
                    users.Add(user); //Blijf users toeveogen
                }

                reader.Close();
                foreach(User u in users)
                {
                    u.Address = GetAddress(u.Id);
                }
                return users; //return alle users
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

        internal ulong GetOrderId(Order order)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                cmd.CommandText = "SELECT * FROM \"order\" where date_time=@date_time and user_id=@user_id and order_status=@order_status";

                cmd.Parameters.AddWithValue("date_time", order.DTime);
                cmd.Parameters.AddWithValue("user_id", (long)order.User.Id);
                cmd.Parameters.AddWithValue("order_status", (int)order.Status);

                NpgsqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    order.Id = (ulong)reader.GetInt32(reader.GetOrdinal("id"));
                }
                reader.Close();
                return order.Id;
                //voeg alle parameters toe an de query en Execute
            }
        }

        internal Order GetOrder(ulong orderId)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                cmd.CommandText = "SELECT * FROM \"order\" where id=@id";

                cmd.Parameters.AddWithValue("id", (long)orderId);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                Order order = new Order();

                if (reader.Read())
                {
                    order.Id = (ulong)reader.GetInt32(reader.GetOrdinal("id"));
                    order.DTime = (DateTime)reader.GetDateTime(reader.GetOrdinal("date_time"));
                    order.User = this.GetUser((ulong)reader.GetInt64(reader.GetOrdinal("user_id")));
                    order.Status = (OrderStatus)Enum.ToObject(typeof(OrderStatus), reader.GetInt64(reader.GetOrdinal("order_status")));
                    order.OrderLines = this.getOrderlines(order.Id);
                }
                reader.Close();
                return order;
                //voeg alle parameters toe an de query en Execute
            }
        }

        internal List<Order> GetOrders()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                cmd.CommandText = "SELECT * FROM \"order\" o join orderline ol on ol.order_id = o.id";

                NpgsqlDataReader reader = cmd.ExecuteReader();
                List<Order> orders = new List<Order>();

                while (reader.Read())
                {
                    Order order = new Order();
                    order.Id = (ulong)reader.GetInt32(reader.GetOrdinal("id"));
                    order.DTime = (DateTime)reader.GetDateTime(reader.GetOrdinal("date_time"));
                    order.User = this.GetUser((ulong)reader.GetInt64(reader.GetOrdinal("user_id")));
                    order.Status = (OrderStatus)Enum.ToObject(typeof(OrderStatus), reader.GetInt64(reader.GetOrdinal("order_status")));
                    order.OrderLines = this.getOrderlines(order.Id);
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


        internal List<OrderLine> getOrderlines(ulong orderId)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                cmd.CommandText = String.Format(@"SELECT * FROM orderline where order_id={0};", (long)orderId);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                List<OrderLine> orderlines = new List<OrderLine>();

                while (reader.Read())
                {
                    OrderLine orderline = new OrderLine();
                    orderline.Product = this.GetProduct((ulong)reader.GetInt64(reader.GetOrdinal("product_id")));
                    orderline.Amount = (ulong)reader.GetInt64(reader.GetOrdinal("amount"));
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

        internal bool UpdateUser(User user)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = this._connection;
                _transaction = cmd.Connection.BeginTransaction();

                cmd.CommandText = "UPDATE \"user\" SET first_name=@first_name, last_name=@last_name, email_address=@email_address, date_of_birth=@date_of_birth where id = @id;";

                cmd.Parameters.AddWithValue("first_name", user.FirstName);
                cmd.Parameters.AddWithValue("last_name", user.LastName);
                cmd.Parameters.AddWithValue("email_address", user.Email);
                cmd.Parameters.AddWithValue("date_of_birth", user.DateOfBirth);
                cmd.Parameters.AddWithValue("id", (long)user.Id);

                //Parameters
                bool success = parseNonqueryResult(cmd.ExecuteNonQuery());
                if (success)
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                    UpdateAddress(user);
                }
                return success;
            }
        }

        internal bool UpdateAddress(User user)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = _connection;
                _transaction = cmd.Connection.BeginTransaction();

                cmd.CommandText = "UPDATE address set street=@street, city=@city, postalcode=@postalcode, \"number\"=@number, suffix=@suffix where postalcode=@postalcode and \"number\"=@number and suffix=@suffix";


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
                return success; //Commit als het sucessvol is
            }
            _transaction.Rollback();
            _transaction.Dispose();
            return success; //Rollback en dispose als het niet lukt
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