using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ETL.Dao.impl
{
    class GetOracleDataDaoImpl : GetOracleDataDao
    {
        public OracleConnection GetOracleConnection()
        {
            XDocument db = XDocument.Load(@"../Resource/dbOracle.xml");
            XElement root = db.Root;
            string username = root.Element("userid").Value;
            string password = root.Element("password").Value;
            string host = root.Element("host").Value;
            string port = root.Element("port").Value;
            string servicename = root.Element("servicename").Value;
            string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + host + ")(PORT=" + port + "))(CONNECT_DATA=(SERVICE_NAME=" + servicename + ")));Persist Security Info=True;User ID=" + username + ";Password=" + password + ";";
            OracleConnection con = new OracleConnection(connectionString);
            return con;
        }

        public DataTable GetDataTableFromOracle(string sql, OracleConnection connection, out bool flag)
        {
           DataTable dt = new DataTable();
           dt.Columns.Add("testcase", typeof(string));
           foreach (DataRow dr in dt.Rows)
           {
               dr["testcase"] = "";
           }
            try
            {
                OracleDataAdapter da = new OracleDataAdapter(sql, connection);
                da.Fill(dt);
                CloseConnection(connection);
                flag = true;
                return dt;
            }
            catch (OracleException)
            {
                flag = false;
                return dt;
            }

        }

        public bool OpenConnection(OracleConnection connection, out string message)
        {
            try
            {
                connection.Open();
                message = "接続成功です";
                return true;
            }
            catch (OracleException ex)
            {
                message = "接続パラメートをチェックして、もう一度やり直してください";
                return false;

            }
        }

        public bool CloseConnection(OracleConnection connection)
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (OracleException ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }
    }
}
