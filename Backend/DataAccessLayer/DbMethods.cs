using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DbMethods
    {
        public MySqlConnection conn
        {
            get
            {
                if (_conn == null)
                {
                    _conn = new MySqlConnection("Enter your sql server link");
                }
                return _conn;
            }
        }

        private MySqlConnection _conn;

        public void OpenConnection()
        {
            MySqlConnection.ClearAllPools();
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw new DALException("Database issue");
                }

            }
        }

        public void CloseConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    conn.Close();
                }
                catch
                {
                    return;
                }
            }
        }
    }
}
