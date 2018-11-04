using System;
using Oracle.ManagedDataAccess.Client;

namespace OracleClient
{
    class Program
    {
        static void Main(string[] args)
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
                {
                    con.Open();

                    // retrieve data
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        cmd.BindByName = true;

                        //Use the command to display employee names from 
                        // the EMPLOYEES table
                        cmd.CommandText = "select first_name from HR.employees where department_id  = :id";

                        // Assign id to the department number 10 
                        OracleParameter id = new OracleParameter("id", 50);
                        cmd.Parameters.Add(id);

                        //Execute the command and use DataReader to display the data
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("Employee Name: " + reader.GetString(0));
                            }
                        }
                    }
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
    }
}