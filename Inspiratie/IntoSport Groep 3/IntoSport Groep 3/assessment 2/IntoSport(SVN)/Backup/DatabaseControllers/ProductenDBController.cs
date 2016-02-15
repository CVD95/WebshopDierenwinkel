using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntoSport.Models;
using MySql.Data.MySqlClient;

namespace IntoSport.DatabaseControllers
{
    public class ProductenDBController : DatabaseController
    {
        public List<Producten> GetProducten()
        {
            List<Producten> producten = new List<Producten>();
            try
            {
                conn.Open();

                string selectQuery = "select productcode, p.naam, merk, producttype, inkoopprijs, verkoopprijs, voorraad, p.aanbiedingscode, omschrijving from product p join aanbieding a on p.aanbiedingscode = a.aanbiedingscode where p.aanbiedingscode > 0";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Producten product = getProductenFromDataReader(dataReader);
                    producten.Add(product);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van aanbiedingen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return producten;
        }

        public List<Producten> GetAlleProductenInList()
        {
            List<Producten> producten = new List<Producten>();
            try
            {
                conn.Open();

                string selectQuery = "select * from product";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Producten product = getProductenFromDataReader(dataReader);
                    producten.Add(product);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van aanbiedingen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return producten;
        }

        public List<Producten> GetListProducten(int productcode)
        {
            List<Producten> producten = new List<Producten>();
            try
            {
                conn.Open();

                string selectQuery = "select* from product where productcode=@productcode";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter productcodeParameter = new MySqlParameter("productcode", MySqlDbType.Int32);
                productcodeParameter.Value = productcode;
                cmd.Parameters.Add(productcodeParameter);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Producten product = getProductenFromDataReader(dataReader);
                    producten.Add(product);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van producten mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return producten;
        }

        public Producten GetProduct(int productcode)
        {
            Producten product = null;

            try
            {
                conn.Open();

                string selectQuery = "select * from product where productcode=@productcode";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter productcodeParameter = new MySqlParameter("productcode", MySqlDbType.Int32);
                productcodeParameter.Value = productcode;
                cmd.Parameters.Add(productcodeParameter);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    product = getProductenFromDataReader(dataReader);
                }

            }
            catch (Exception e)
            {

                Console.Write("Ophalen van Product mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return product;
        }

        public List<MaatModel> GetMaat()
        {
            List<MaatModel> model = new List<MaatModel>();
            try
            {
                conn.Open();

                string selectQuery = @"SELECT * FROM product_maat
                                        group by grootte";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    MaatModel maat = getMatenFromDataReader(dataReader);
                    model.Add(maat);
                }
            }
            catch (Exception e)
            {

                Console.Write("Ophalen van Product mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return model;
        }
        public string GetMaatGrootte(int id)
        {
            string maat = null;
            try
            {
                conn.Open();

                string selectQuery = @"SELECT grootte from product_maat where id=@id";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter idParam = new MySqlParameter("id", MySqlDbType.Int32);
                idParam.Value = id;
                cmd.Parameters.Add(idParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    maat = getMaatGrootteFromDataReader(dataReader);
                }
                return maat;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public int GetInsertId()
        {
            int id = 0;

            try
            {
                conn.Open();

                string getQuery = @"select max(productcode) as productcode from product";

                MySqlCommand cmd = new MySqlCommand(getQuery, conn);

                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    id = GetProductIdFromDataReader(dataReader);
                }
            }
            catch (Exception e)
            {
                // Moet naar error page, niet alleen throw e
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return id;
        }
        
        public bool InsertProductEnInkoop(Producten product, int aanbieding)
        {
            // Geen Transaction, of we moeten nieuwe reference "Transaction" gebruiken met TransactionScope.
            int id;

            try
            {
                // Insert Product in tabel product
                InsertProduct(product, aanbieding);

                id = GetInsertId();

                // Check of id 0 is.
                if (id == 0)
                    return false;

                // Get Maten voor kledingstuk
                // ATTENTIE: Moet checken of heer / vrouw is!(Extra)
                List<MaatModel> maat = GetMaat();

                // Insert Product in tabel product_inkoop voor verschillende voorraden per maat. (ALLE MATEN HEBBEN DUS DEZELFDE INITIELE VOORRAAD!)
                for (int i = 0; i < maat.Count; i++)
                {
                    InsertProductInkoop(product, id, maat[i].grootte);
                }
            }
            catch (Exception e)
            {
                VerwijderProduct( GetInsertId());
                // VerwijderProductInkoop(id); // ON CASCADE UPDATE / DELETE
                return false;
                throw e;
            }
            return true;

        }
        public void InsertProductInkoop(Producten product/*Product Voorraad*/, int id /*FK productcode*/, string maat /*Maat*/)
        {
            try
            { //open transactie en maak insert string en verbinding
                conn.Open();

                string insertString = @"insert into product_inkoop (voorraad, maat, productcode) " +
                                                       " values (@voorraad, @maat,  @productcode)";

                //commando en prepared statement
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter voorraadParameter = new MySqlParameter("@voorraad", MySqlDbType.Int32);
                MySqlParameter maatParameter = new MySqlParameter("@maat", MySqlDbType.String);
                MySqlParameter productcodeParameter = new MySqlParameter("@productcode", MySqlDbType.Int32);

                //set parameters naar view Waarden
                voorraadParameter.Value = product.productVoorraad;
                maatParameter.Value = maat;
                productcodeParameter.Value = id;

                //voeg parameters toe
                cmd.Parameters.Add(voorraadParameter);
                cmd.Parameters.Add(maatParameter);
                cmd.Parameters.Add(productcodeParameter);

                //prepare Execute Commit
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {// is er een fout?
                Console.Write("Product niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public void InsertProduct(Producten product, int aanbieding)
        {
            try
            { //open transactie en maak insert string en verbinding
                conn.Open();

                string insertString = @"insert into product (naam, merk, inkoopprijs, verkoopprijs, voorraad , aanbiedingscode  , producttype, omschrijving) " +
                                                       " values (@naam, @merk,  @inkoopprijs, @verkoopprijs, @voorraad , @aanbiedingscode , @producttype, @omschrijving)";

                //commando en prepared statement
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter naamparameter = new MySqlParameter("@naam", MySqlDbType.String);
                MySqlParameter merkparameter = new MySqlParameter("@merk", MySqlDbType.String);
                MySqlParameter inkoopprijsparameter = new MySqlParameter("@inkoopprijs", MySqlDbType.Double);
                MySqlParameter verkoopprijsparameter = new MySqlParameter("@verkoopprijs", MySqlDbType.Double);
                MySqlParameter voorraadparameter = new MySqlParameter("@voorraad", MySqlDbType.Int32);
                MySqlParameter aanbiedingscodeparameter = new MySqlParameter("@aanbiedingscode", MySqlDbType.Int32);
                MySqlParameter producttypeparameter = new MySqlParameter("@producttype", MySqlDbType.String);
                MySqlParameter omschrijvingParameter = new MySqlParameter("@omschrijving", MySqlDbType.String);

                //set parameters naar view Waarden
                naamparameter.Value = product.productNaam;
                merkparameter.Value = product.productMerk;
                inkoopprijsparameter.Value = product.productInkoopprijs;
                verkoopprijsparameter.Value = product.productVerkoopprijs;
                voorraadparameter.Value = product.productVoorraad;
                aanbiedingscodeparameter.Value = aanbieding;
                producttypeparameter.Value = product.productType;
                omschrijvingParameter.Value = product.productOmschrijving;

                //voeg parameters toe
                cmd.Parameters.Add(naamparameter);
                cmd.Parameters.Add(merkparameter);
                cmd.Parameters.Add(inkoopprijsparameter);
                cmd.Parameters.Add(verkoopprijsparameter);
                cmd.Parameters.Add(voorraadparameter);
                cmd.Parameters.Add(aanbiedingscodeparameter);
                cmd.Parameters.Add(producttypeparameter);
                cmd.Parameters.Add(omschrijvingParameter);

                //prepare Execute Commit
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {// is er een fout?
                Console.Write("Product niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void InsertProduct(Producten product)
        {
            try
            { //open transactie en maak insert string en verbinding
                conn.Open();

                string insertString = @"insert into product (naam, merk, inkoopprijs, verkoopprijs, voorraad , aanbiedingscode , producttype, omschrijving) " +
                                                       " values (@naam, @merk,  @inkoopprijs, @verkoopprijs, @voorraad , @aanbiedingscode, @producttype, @omschrijving)";

                //commando en prepared statement
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter naamparameter = new MySqlParameter("@naam", MySqlDbType.String);
                MySqlParameter merkparameter = new MySqlParameter("@merk", MySqlDbType.String);
                MySqlParameter inkoopprijsparameter = new MySqlParameter("@inkoopprijs", MySqlDbType.Double);
                MySqlParameter verkoopprijsparameter = new MySqlParameter("@verkoopprijs", MySqlDbType.Double);
                MySqlParameter voorraadparameter = new MySqlParameter("@voorraad", MySqlDbType.Int32);
                MySqlParameter aanbiedingscodeparameter = new MySqlParameter("@aanbiedingscode", MySqlDbType.Int32);
                MySqlParameter producttypeparameter = new MySqlParameter("@producttype", MySqlDbType.String);
                MySqlParameter omschrijvingParameter = new MySqlParameter("@omschrijving", MySqlDbType.String);

                //set parameters naar view Waarden
                naamparameter.Value = product.productNaam;
                merkparameter.Value = product.productMerk;
                inkoopprijsparameter.Value = product.productInkoopprijs;
                verkoopprijsparameter.Value = product.productVerkoopprijs;
                voorraadparameter.Value = product.productVoorraad;
                aanbiedingscodeparameter.Value = product.productAanbiedingscode;
                producttypeparameter.Value = product.productType;
                omschrijvingParameter.Value = product.productOmschrijving;

                //voeg parameters toe
                cmd.Parameters.Add(naamparameter);
                cmd.Parameters.Add(merkparameter);
                cmd.Parameters.Add(inkoopprijsparameter);
                cmd.Parameters.Add(verkoopprijsparameter);
                cmd.Parameters.Add(voorraadparameter);
                cmd.Parameters.Add(aanbiedingscodeparameter);
                cmd.Parameters.Add(producttypeparameter);
                cmd.Parameters.Add(omschrijvingParameter);

                //prepare Execute Commit
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {// is er een fout?
                Console.Write("Product niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateProduct(Producten product)
        {
            try
            {
                conn.Open();

                string insertString = @"UPDATE product " +
                                        "SET naam=@productnaam, merk=@productmerk, inkoopprijs=@productinkoopprijs, verkoopprijs=@productverkoopprijs, voorraad=@productvoorraad, aanbiedingscode=@productaanbiedingscode, producttype=@producttype, omschrijving=@productomschrijving " +
                                         "WHERE productcode=@productId";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter productcodeParameter = new MySqlParameter("@productId", MySqlDbType.Int32);
                MySqlParameter productnaamParameter = new MySqlParameter("@productnaam", MySqlDbType.String);
                MySqlParameter productmerkParameter = new MySqlParameter("@productmerk", MySqlDbType.String);
                MySqlParameter productinkoopprijsParameter = new MySqlParameter("@productinkoopprijs", MySqlDbType.Double);
                MySqlParameter productverkoopprijsParameter = new MySqlParameter("@productverkoopprijs", MySqlDbType.Double);
                MySqlParameter productvoorraadParameter = new MySqlParameter("@productvoorraad", MySqlDbType.Int32);
                MySqlParameter productaanbiedingscodeParameter = new MySqlParameter("@productaanbiedingscode", MySqlDbType.Int32);
                MySqlParameter productmaatParameter = new MySqlParameter("@productmaat", MySqlDbType.String);
                MySqlParameter producttypeParameter = new MySqlParameter("@producttype", MySqlDbType.String);
                MySqlParameter productOmschrijvingParameter = new MySqlParameter("@productomschrijving", MySqlDbType.String);

                productnaamParameter.Value = product.productNaam;
                productcodeParameter.Value = product.productCode;
                productmerkParameter.Value = product.productMerk;
                productinkoopprijsParameter.Value = product.productInkoopprijs;
                productverkoopprijsParameter.Value = product.productVerkoopprijs;
                productvoorraadParameter.Value = product.productVoorraad;
                productaanbiedingscodeParameter.Value = product.productAanbiedingscode;
                //productmaatParameter.Value = product.productMaat;
                producttypeParameter.Value = product.productType;
                productOmschrijvingParameter.Value = product.productOmschrijving;

                cmd.Parameters.Add(productcodeParameter);
                cmd.Parameters.Add(productnaamParameter);
                cmd.Parameters.Add(productmerkParameter);
                cmd.Parameters.Add(productinkoopprijsParameter);
                cmd.Parameters.Add(productverkoopprijsParameter);
                cmd.Parameters.Add(productvoorraadParameter);
                cmd.Parameters.Add(productaanbiedingscodeParameter);
                cmd.Parameters.Add(productmaatParameter);
                cmd.Parameters.Add(producttypeParameter);
                cmd.Parameters.Add(productOmschrijvingParameter);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Updaten product niet gelukt: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

        }
        public Boolean UpdateProductEnInkoopVoorraad(Producten product, int aanbiedingscode)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                UpdateProduct(product, aanbiedingscode);
                UpdateInkoopVoorraad(product.productCode, product.productVoorraad);

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return true;
        }

        public void UpdateInkoopVoorraad(int id, int voorraad)
        {
            try
            {
                string updateQuery = @"UPDATE product_inkoop
                                       SET voorraad=@voorraad
                                       WHERE productcode=@id";

                MySqlCommand cmd = new MySqlCommand(updateQuery, conn);

                MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32);
                MySqlParameter voorraadParam = new MySqlParameter("@voorraad", MySqlDbType.Int32);

                idParam.Value = id;
                voorraadParam.Value = voorraad;

                cmd.Parameters.Add(idParam);
                cmd.Parameters.Add(voorraadParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public void UpdateProduct(Producten product, int aanbiedingscode)
        {
            try
            {

                string insertString = @"UPDATE product
                                        SET naam=@productnaam, merk=@productmerk, inkoopprijs=@productinkoopprijs, verkoopprijs=@productverkoopprijs, voorraad=@productvoorraad, aanbiedingscode=@productaanbiedingscode, producttype=@producttype, omschrijving=@productomschrijving
                                         WHERE productcode=@productId";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter productcodeParameter = new MySqlParameter("@productId", MySqlDbType.Int32);
                MySqlParameter productnaamParameter = new MySqlParameter("@productnaam", MySqlDbType.String);
                MySqlParameter productmerkParameter = new MySqlParameter("@productmerk", MySqlDbType.String);
                MySqlParameter productinkoopprijsParameter = new MySqlParameter("@productinkoopprijs", MySqlDbType.Double);
                MySqlParameter productverkoopprijsParameter = new MySqlParameter("@productverkoopprijs", MySqlDbType.Double);
                MySqlParameter productvoorraadParameter = new MySqlParameter("@productvoorraad", MySqlDbType.Int32);
                MySqlParameter productaanbiedingscodeParameter = new MySqlParameter("@productaanbiedingscode", MySqlDbType.Int32);
                MySqlParameter producttypeParameter = new MySqlParameter("@producttype", MySqlDbType.String);
                MySqlParameter productOmschrijvingParameter = new MySqlParameter("@productomschrijving", MySqlDbType.String);

                productnaamParameter.Value = product.productNaam;
                productcodeParameter.Value = product.productCode;
                productmerkParameter.Value = product.productMerk;
                productinkoopprijsParameter.Value = product.productInkoopprijs;
                productverkoopprijsParameter.Value = product.productVerkoopprijs;
                productvoorraadParameter.Value = product.productVoorraad;
                productaanbiedingscodeParameter.Value = aanbiedingscode;
                producttypeParameter.Value = product.productType;
                productOmschrijvingParameter.Value = product.productOmschrijving;

                cmd.Parameters.Add(productcodeParameter);
                cmd.Parameters.Add(productnaamParameter);
                cmd.Parameters.Add(productmerkParameter);
                cmd.Parameters.Add(productinkoopprijsParameter);
                cmd.Parameters.Add(productverkoopprijsParameter);
                cmd.Parameters.Add(productvoorraadParameter);
                cmd.Parameters.Add(productaanbiedingscodeParameter);
                cmd.Parameters.Add(producttypeParameter);
                cmd.Parameters.Add(productOmschrijvingParameter);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Updaten product niet gelukt: " + e);
                throw e;
            }

        }

        public void VerwijderProduct(int productcode)
        {
            try
            {
                conn.Open();

                string deleteQuery = @"delete from product where productcode=@id";

                MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                MySqlParameter idCodeParam = new MySqlParameter("@id", MySqlDbType.Int32);

                idCodeParam.Value = productcode;

                cmd.Parameters.Add(idCodeParam);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Product niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        // CASCADE!
        public void VerwijderProductInkoop(int productcode)
        {
            try
            {
                conn.Open();

                string deleteQuery = @"DELETE from product WHERE productcode=@id";

                MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32);

                idParam.Value = productcode;
                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public Aanbieding GetAanbieding(int aanbiedingscode)
        {
            Aanbieding aanbieding = null;

            try
            {
                conn.Open();

                string selectQuery = "select * from aanbieding where aanbiedingscode=@ab";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter productcodeParameter = new MySqlParameter("ab", MySqlDbType.Int32);
                productcodeParameter.Value = aanbiedingscode;
                cmd.Parameters.Add(productcodeParameter);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    aanbieding = GetAanbiedingAangepastFromDataReader(dataReader);
                }

            }
            catch (Exception e)
            {

                Console.Write("Ophalen van Product mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return aanbieding;
        }

        public List<MaatModel> getMaten()
        {


            List<MaatModel> maten = new List<MaatModel>();
            try
            {
                conn.Open();

                string selectQuery = "select * from product_maat";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    MaatModel maat = getMatenFromDataReader(dataReader);
                    maten.Add(maat);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van aanbiedingen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return maten;


        }

        public MaatModel GetMaat(int maatcode)
        {
            MaatModel maat = null;

            try
            {
                conn.Open();

                string selectQuery = "select * from maat where categorie_code=@maatcode";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter productcodeParameter = new MySqlParameter("maatcode", MySqlDbType.Int32);
                productcodeParameter.Value = maatcode;
                cmd.Parameters.Add(productcodeParameter);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    maat = getMatenFromDataReader(dataReader);
                }

            }
            catch (Exception e)
            {

                Console.Write("Ophalen van Product mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return maat;
        }

        public List<Producten> SearchProducten(string Zoekresultaten, string Criteria)
        {

            List<Producten> producten = new List<Producten>();

            try
            {
                conn.Open();

                string selectQuery = "SELECT productcode, naam, merk, inkoopprijs, verkoopprijs, voorraad, producttype, aanbiedingscode, omschrijving FROM product " +
                " WHERE " + Criteria + " LIKE '%" + Zoekresultaten + "%' " +
                " LIMIT 10 ;";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Producten product = getProductenFromDataReader(dataReader);
                    producten.Add(product);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van Producten mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return producten;

        }

    }
}
