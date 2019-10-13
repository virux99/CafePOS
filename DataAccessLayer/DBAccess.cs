using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    class DBAccess
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MainDB.mdf;Integrated Security=True");
       public void OpenConnection()
        {
            conn.Open();
        }
        public void CloseConnection()
        {
            conn.Close();
        }
        public void ExecuteQuery(string query, Hashtable parameters)
        {
            OpenConnection();
            SqlCommand cmd = new SqlCommand(query,conn);
            foreach (DictionaryEntry parameter in parameters)
            {
                cmd.Parameters.AddWithValue(parameter.Key.ToString(), parameter.Value);

            }
            cmd.ExecuteNonQuery();
            CloseConnection();
        }
        public int ExecuteIDQuery(string query, Hashtable parameters)
        {
            OpenConnection();
            SqlCommand cmd = new SqlCommand(query,conn);
            foreach (DictionaryEntry parameter in parameters)
            {
                cmd.Parameters.AddWithValue(parameter.Key.ToString(), parameter.Value);

            }
            int id=  Convert.ToInt32 (cmd.ExecuteScalar());
                    CloseConnection();
            return id;
        }


        public void ExecuteQuery(string query)
        {
            OpenConnection();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }
        public DataTable GetDataTable(string query)
        {
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}
