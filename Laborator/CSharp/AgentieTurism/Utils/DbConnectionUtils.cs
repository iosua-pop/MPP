using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AgentieTurism.Utils
{
    public class DbConnectionUtils
    {
        private static IDbConnection instance = null;

        public static IDbConnection GetConnection()
        {
            if (instance == null || instance.State == ConnectionState.Closed)
            {
                instance = GetNewConnection();
                instance.Open();
            }
            return instance;
        }

        private static IDbConnection GetNewConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            Console.WriteLine($"Opening a new SQLite connection to: {connectionString}");
            return new SQLiteConnection(connectionString);
        }
    }
}
