﻿using System;
using Oracle.ManagedDataAccess.Client;

namespace OracleClient
{
    class Program
    {

        private const string DropSchemeSql = @"
BEGIN
  FOR rec IN
    (
      SELECT
        table_name
      FROM
        all_tables
      WHERE
        table_name In ('SALGRADE', 'BONUS', 'EMP', 'DEPT')
    )
  LOOP
    EXECUTE immediate 'DROP TABLE  '||rec.table_name || ' CASCADE CONSTRAINTS';
  END LOOP;
END;
/
";

        private const string CreateSchemaSql = @"
CREATE TABLE DEPT
       (DEPTNO NUMBER(2) CONSTRAINT PK_DEPT PRIMARY KEY,
	DNAME VARCHAR2(14) ,
	LOC VARCHAR2(13) ) ;
    
CREATE TABLE EMP
       (EMPNO NUMBER(4) CONSTRAINT PK_EMP PRIMARY KEY,
	ENAME VARCHAR2(10),
	JOB VARCHAR2(9),
	MGR NUMBER(4),
	HIREDATE DATE,
	SAL NUMBER(7,2),
	COMM NUMBER(7,2),
	DEPTNO NUMBER(2) CONSTRAINT FK_DEPTNO REFERENCES DEPT);
    

CREATE TABLE BONUS
	(
	ENAME VARCHAR2(10)	,
	JOB VARCHAR2(9)  ,
	SAL NUMBER,
	COMM NUMBER
	) ;

CREATE TABLE SALGRADE
      ( GRADE NUMBER,
	LOSAL NUMBER,
	HISAL NUMBER );
";

        private const string InsertDataSql = @"
INSERT INTO DEPT VALUES
	(10,'ACCOUNTING','NEW YORK');
INSERT INTO DEPT VALUES (20,'RESEARCH','DALLAS');
INSERT INTO DEPT VALUES
	(30,'SALES','CHICAGO');
INSERT INTO DEPT VALUES
	(40,'OPERATIONS','BOSTON');
    
INSERT INTO EMP VALUES
(7369,'SMITH','CLERK',7902,to_date('17-12-1980','dd-mm-yyyy'),800,NULL,20);
INSERT INTO EMP VALUES
(7499,'ALLEN','SALESMAN',7698,to_date('20-2-1981','dd-mm-yyyy'),1600,300,30);
INSERT INTO EMP VALUES
(7521,'WARD','SALESMAN',7698,to_date('22-2-1981','dd-mm-yyyy'),1250,500,30);
INSERT INTO EMP VALUES
(7566,'JONES','MANAGER',7839,to_date('2-4-1981','dd-mm-yyyy'),2975,NULL,20);
INSERT INTO EMP VALUES
(7654,'MARTIN','SALESMAN',7698,to_date('28-9-1981','dd-mm-yyyy'),1250,1400,30);
INSERT INTO EMP VALUES
(7698,'BLAKE','MANAGER',7839,to_date('1-5-1981','dd-mm-yyyy'),2850,NULL,30);
INSERT INTO EMP VALUES
(7782,'CLARK','MANAGER',7839,to_date('9-6-1981','dd-mm-yyyy'),2450,NULL,10);
INSERT INTO EMP VALUES
(7788,'SCOTT','ANALYST',7566,to_date('13-JUL-87')-85,3000,NULL,20);
INSERT INTO EMP VALUES
(7839,'KING','PRESIDENT',NULL,to_date('17-11-1981','dd-mm-yyyy'),5000,NULL,10);
INSERT INTO EMP VALUES
(7844,'TURNER','SALESMAN',7698,to_date('8-9-1981','dd-mm-yyyy'),1500,0,30);
INSERT INTO EMP VALUES
(7876,'ADAMS','CLERK',7788,to_date('13-JUL-87')-51,1100,NULL,20);
INSERT INTO EMP VALUES
(7900,'JAMES','CLERK',7698,to_date('3-12-1981','dd-mm-yyyy'),950,NULL,30);
INSERT INTO EMP VALUES
(7902,'FORD','ANALYST',7566,to_date('3-12-1981','dd-mm-yyyy'),3000,NULL,20);
INSERT INTO EMP VALUES
(7934,'MILLER','CLERK',7782,to_date('23-1-1982','dd-mm-yyyy'),1300,NULL,10);
    
INSERT INTO SALGRADE VALUES (1,700,1200);
INSERT INTO SALGRADE VALUES (2,1201,1400);
INSERT INTO SALGRADE VALUES (3,1401,2000);
INSERT INTO SALGRADE VALUES (4,2001,3000);
INSERT INTO SALGRADE VALUES (5,3001,9999);
";

        private const string SetupSql = DropSchemeSql + CreateSchemaSql + InsertDataSql;

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

                    // note: script `SetupSql` works fine when run from SQL Developer UI but fails with:
                    // PLS-00103: Encountered the symbol "/"
/*
                    // setup db
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        cmd.BindByName = true;

                        // Create and populate db tables with demo data
                        cmd.CommandText = SetupSql;
                        cmd.ExecuteNonQuery();

                        Console.WriteLine();
                        Console.WriteLine("Done populating db with demo data");
                    }
*/

                    // retrieve data
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        cmd.BindByName = true;

                        //Use the command to display employee names from 
                        // the EMPLOYEES table
                        cmd.CommandText = "select ENAME from EMP where DEPTNO = :id";

                        // Assign id to the department number 10 
                        OracleParameter id = new OracleParameter("id", 10);
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