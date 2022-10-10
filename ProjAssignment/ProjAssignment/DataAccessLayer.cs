using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        
        public DataTable ReadByStoredProcedure(string spName)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand(spName, connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();

            var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);


            return table;
        }

        public void AddCaretaker (string name, string phoneNumber)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand("usp_CreateCareTaker", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@phoneNbr", phoneNumber);
            connection.Open();
            command.ExecuteNonQuery();

        }
        public void CallProcedureWithParameters(string[] paramNames, object?[] values, string spName)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand(spName, connection);
            command.CommandType = CommandType.StoredProcedure;

            if(paramNames.Length == values.Length)
            {
                for(int i = 0; i < paramNames.Length; i++)
                {
                    command.Parameters.AddWithValue(paramNames[i], values[i]);
                }
                connection.Open();
                command.ExecuteNonQuery();
            }

        
            

        }



        public string ConnectionString => connectionString;

    }
}
