using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjAssignment
{
    internal class ErrorHandling
    {
        public static string SqlError(SqlException ex)
        {
            if (ex.Number == 51000)
            {
                var msg = ex.Message;

                if (msg.Contains("Enclosures"))
                {
                    return "Selected enclosure not found";


                }

                if (msg.Contains("Foods"))
                {
                    return "Selected food not found";

                }

            }
            return "Something went wrong";
        }
    }
}
