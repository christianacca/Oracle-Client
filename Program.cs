using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace Oracle_Client
{
    class Program
    {
        static async Task Main()
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
                    // note: you will need to wait for the oracledb docker service (aka container)
                    // to become healthy... until then opening connection will fail
                    await con.OpenAsync();

                    const string selectSql =
                        "select first_name AS FirstName from HR.employees where department_id  = :id";

                    cmd.BindByName = true;
                    cmd.CommandText = selectSql;
                    cmd.Parameters.Add(new OracleParameter("id", 10));

                    await FetchDataUsingDataReader(cmd);

                    await FetchQuerySchema(cmd);

                    await FetchDataUsingDapper(con, selectSql);
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

        private static async Task FetchDataUsingDapper(OracleConnection con, string selectSql)
        {
            var rows = await con.QueryAsync(selectSql, new {id = 10});

            Console.WriteLine("Fetch data using Dapper (dyamic)");
            foreach (var row in rows)
            {
                Console.WriteLine("Employee Name: " + row.FIRSTNAME);
            }
        }

        private static async Task FetchDataUsingDataReader(DbCommand command)
        {
            //Execute the command and use DataReader to display the data
            using (var reader = await command.ExecuteReaderAsync())
            {
                Console.WriteLine("Fetch data using DataReader");
                while (reader.Read())
                {
                    Console.WriteLine("Employee Name: " + reader.GetString(0));
                }
            }
        }

        private static async Task FetchQuerySchema(DbCommand command)
        {
            using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SchemaOnly))
            {
                Console.WriteLine("Schema information");

                IEnumerable<ColumnSchemaInfo> columns = reader.GetSchemaInfo();
                columns.ToList().ForEach(Console.Out.WriteLine);

                // the raw metadata (as an alternative to consuming ColumnSchemaInfo's)
                var schemaRecords = reader.GetSchemaInfoRecords().ToList();
                foreach (var (columnName, key, value) in schemaRecords)
                {
                    Console.Out.WriteLine($"{columnName}: {key} = {value}");
                }
            }
        }
    }
}