using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ProjAssignment
{
    internal class DataAccessLayer
    {
        private readonly string connectionString;
       

        public DataAccessLayer()
        {
            var s = ConfigurationManager.ConnectionStrings["IllegalSkillsException"].ConnectionString;
            if (s != null)
            {
                connectionString = s;
            }
            else
            {
                connectionString = "";
            }
            
        }

        public void TryConnection()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand command = new SqlCommand("SELECT * FROM Animal",connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Debug.WriteLine("{0}\t{1}", reader.GetInt32(0),
                      reader.GetString(1));
            }
            connection.Close();
        }
        public string ConnectionString => connectionString;
    }
}
