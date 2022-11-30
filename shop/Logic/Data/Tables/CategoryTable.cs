using MySql.Data.MySqlClient;
using Shop.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Logic.Data.Tables
{
    internal class CategoryTable : Table
    {

        private static readonly string s_table = "categories", s_id = "category_id", s_name = "category_name";
        public CategoryTable() : base(s_table, s_id)
        {
        }

        public int Create(string name)
        {
            return Insert(name);
        }

        public List<string> Get()
        {
            List<string> list = new List<string>();
            MySqlDataReader reader = Select();
            while (reader.Read())
            {
                list.Add(reader.GetString(s_name));
            }
            reader.Close();
            return list;
        }

        public string Get(int id)
        {
            MySqlDataReader reader = Select(new Condition(new DataField(s_id, id)));
            reader.Read();
            string name = reader.GetString(s_name);
            reader.Close();
            return name;
        }

        public int Get(string name)
        {
            MySqlDataReader reader = Select(new Condition(new DataField(s_name, name)));
            reader.Read();
            int id = reader.GetInt32(s_id);
            reader.Close();
            return id;
        }

        public void Update(int id, string newName)
        {
            Condition con = new Condition(new DataField(s_id, id));
            Update(new DataField(s_name, newName), con);
        }

        public void Remove(int id)
        {
            Delete(new Condition(new DataField(s_id, id)));
        }
    }
}
