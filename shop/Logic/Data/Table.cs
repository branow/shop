using MySql.Data.MySqlClient;

namespace Shop.Logic.Data
{
    class Table
    {

        protected int Id { get; set; }
        public string Name { get; set; }
        public string IdColumn { get; set; }

        public Table(string tableName, string idColumn) {
            Name = tableName;
            IdColumn = idColumn;
            Id = GetId();
        }

        protected int Insert(params object[] objects) 
        {
            int id = Id++;
            string com = $"insert into {Name} values(@id, {GetParamersString(objects.Length)})";
            MySqlCommand command = new MySqlCommand(com, DataBase.Connection());
            command.Parameters.AddWithValue("@id", id);
            for (int i = 0; i < objects.Length; i++)
            {
                command.Parameters.AddWithValue($"@o{i}", objects[i]);
            }
            command.ExecuteNonQuery();
            return id;
        }

        protected MySqlDataReader Select(Condition condition)
        {
            string com = $"select * from {Name} {condition.ToString("@o")}";
            MySqlCommand command = new MySqlCommand(com, DataBase.Connection());
            for (int i = 0; i < condition.Fields.Length; i++)
            {
                command.Parameters.AddWithValue($"@o{i}", condition.Fields[i].Value);
            }
            return command.ExecuteReader();
        }

        protected MySqlDataReader Select()
        {
            string com = $"select * from {Name}";
            MySqlCommand command = new MySqlCommand(com, DataBase.Connection());
            return command.ExecuteReader();
        }

        protected void Update(DataField field, Condition condition)
        {
            string com = $"update {Name} set {field.ToString("@val")} {condition}";
            MySqlCommand command = new MySqlCommand(com, DataBase.Connection());
            command.Parameters.AddWithValue("@val", field.Value);
            for (int i = 0; i < condition.Fields.Length; i++)
            {
                command.Parameters.AddWithValue($"@o{i}", condition.Fields[i].Value);
            }
            command.ExecuteNonQuery();
        }

        protected void Delete(Condition condition)
        {
            string com = $"delete from {Name} {condition}";
            MySqlCommand command = new MySqlCommand(com, DataBase.Connection());
            for (int i = 0; i < condition.Fields.Length; i++)
            {
                command.Parameters.AddWithValue($"@o{i}", condition.Fields[i].Value);
            }
            command.ExecuteNonQuery();
        }


        private string GetParamersString(int size) 
        {
            string param = "";
            for (int i = 0; i < size; i++) 
            {
                param += "@o" + i;
                if (i < size - 1)
                    param += ", ";
            }
            return param;
        }

        private int GetId() 
        {
            string com = $"select max({IdColumn}) from {Name} ";
            MySqlCommand command = new MySqlCommand(com, DataBase.Connection());
            MySqlDataReader reader = command.ExecuteReader();
            int id;
            reader.Read();
            try
            {
                id = reader.GetInt32(0);
            }
            catch (Exception e)
            {
                id = -1;
            }
            reader.Close();
            return  id+1;
        }


        


    }
}
