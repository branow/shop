using MySql.Data.MySqlClient;
using Shop.Logic;
using Shop.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Logic.Data.Tables
{
    class ProductTable : Table
    {

        private static readonly string s_table = "products", s_id = "product_id", s_name = "product_name",
            s_price = "price", s_category = "category", s_description = "product_description", s_seller = "seller_id";
        public ProductTable() : base(s_table, s_id)
        {
        }

        public Product Create(string name, decimal price, int categoryId, int sellerId, string description = "")
        {
            int id = Insert(name, price, categoryId, description, sellerId);
            return new Product(id, name, price, categoryId, description, sellerId);
        }

        public List<Product> Get()
        {
            List<Product> list = new List<Product>();
            MySqlDataReader reader = Select();
            while (reader.Read())
            {
                Product product = new Product(reader.GetInt32(s_id), reader.GetString(s_name), reader.GetDecimal(s_price),
                        reader.GetInt32(s_category), reader.GetString(s_description), reader.GetInt32(s_seller));
                list.Add(product);
            }
            reader.Close();
            return list;
        }

        public Product Get(int id)
        {
            MySqlDataReader reader = Select(new Condition(new DataField(s_id, id)));
            reader.Read();
            Product product = new Product(reader.GetInt32(s_id), reader.GetString(s_name), reader.GetDecimal(s_price),
                       reader.GetInt32(s_category), reader.GetString(s_description), reader.GetInt32(s_seller));
            reader.Close();
            return product;
        }

        public void Update(Product product)
        {
            Condition con = new Condition(new DataField(s_id, product.Id));
            Update(new DataField(s_name, product.Name), con);
            Update(new DataField(s_price, product.Price), con);
            Update(new DataField(s_category, product.CategoryId), con);
            Update(new DataField(s_description, product.Description), con);
            Update(new DataField(s_seller, product.SellerId), con);

        }

        public void Remove(int id)
        {
            Delete(new Condition(new DataField(s_id, id)));
        }
    }
}
