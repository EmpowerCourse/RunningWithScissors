using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningWithScissors
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.AppSettings["ConnectionString"];

            Console.WriteLine("Enter a table name:");
            var tableName = Console.ReadLine();
            var sqlSelect = $"select * from {tableName}";
            Console.WriteLine($"This is the query that is about to run: {sqlSelect}");
            var row = 0;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var command = new SqlCommand(sqlSelect, sqlConnection))
                {
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            row++;
                            Console.Write($"Data from row {row}: ");
                            var values = new List<string>();
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                values.Add(dr[i] == DBNull.Value ? "NULL" : dr[i].ToString());
                            }
                            Console.WriteLine(string.Join("|", values));
                        }
                    }
                }
            }

            Console.WriteLine($"You had {row}{(row > 0 ? "s" : "")} of data");
            Console.ReadKey();
        }
    }
}
