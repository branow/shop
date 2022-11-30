using MySql.Data.MySqlClient;

namespace Shop.Logic.Data
{
    class DataBase
    {
        private static string s_config = "server=localhost;port=3306;username=useroot;password=root;database=shop";
        private static readonly MySqlConnection s_connection = new MySqlConnection(s_config);

        public static MySqlConnection Connection()
        {
            Open();
            return s_connection;
        }

        public static void Open() 
        { 
           if (s_connection.State == System.Data.ConnectionState.Closed)
                s_connection.Open();
        }

        public static void Close()
        {
            if (s_connection.State == System.Data.ConnectionState.Open)
                s_connection.Close();
        }



    }
}
