using System;
using System.Data;
using System.Linq;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace Oracle_Client
{
    class Program
    {
        static void Main()
        {
            //Demo: Basic ODP.NET Core application to connect, query, and return
            // results from an OracleDataReader to a console

            //Create a connection to Oracle			
            string conString = "User Id=sys;Password=Oracle18;DBA Privilege=SYSDBA;" +

            //How to connect to an Oracle DB without SQL*Net / tnsnames.ora configuration file
            "Data Source=oracledb:1521/XEPDB1;";

            // use the following when the docker container for db has published it's port to the host
//            "Data Source=localhost:32118/XEPDB1;";

            //How to connect to an Oracle DB with a DB alias.
            //Uncomment below and comment above.
            //"Data Source=<service name alias>;";

            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                using(OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();

                    const string selectSql =
                        "select first_name AS FirstName from HR.employees where department_id  = :id";

                    cmd.BindByName = true;
                    cmd.CommandText = selectSql;
                    cmd.Parameters.Add(new OracleParameter("id", 10));

                    FetchDataUsingDataReader(cmd);

                    FetchQuerySchema(cmd);

                    FetchDataUsingDapper(con, selectSql);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Press 'Enter' to continue");

            Console.ReadLine();
        }

        private static void FetchDataUsingDapper(OracleConnection con, string selectSql)
        {
            var rows = con.Query(selectSql, new {id = 10}).ToList();

            Console.WriteLine("Fetch data using Dapper (dyamic)");
            foreach (var row in rows)
            {
                Console.WriteLine("Employee Name: " + row.FIRSTNAME);
            }
        }

        private static void FetchDataUsingDataReader(OracleCommand command)
        {
            //Execute the command and use DataReader to display the data
            using (OracleDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Fetch data using DataReader");
                while (reader.Read())
                {
                    Console.WriteLine("Employee Name: " + reader.GetString(0));
                }
            }
        }

        private static void FetchQuerySchema(OracleCommand command)
        {
            using (OracleDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
            {
                var schemaInfo = reader.GetSchemaInfo();

                Console.WriteLine("Schema information");
                foreach (var (columnName, key, value) in schemaInfo)
                {
                    Console.Out.WriteLine($"{columnName}: {key} = {value}");
                }
            }
        }
    }
}