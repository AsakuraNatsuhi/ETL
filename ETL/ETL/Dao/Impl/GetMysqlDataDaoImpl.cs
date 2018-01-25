using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ETL.Dao.impl
{
    class GetMysqlDataDaoImpl : GetMysqlDataDao
    {
        public DataTable GetDataTableFromMysql(string sql, MySqlConnection connection,out bool flag)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("testcase",typeof(string));
            foreach (DataRow dr in dt.Rows)
            {
                dr["testcase"] = "";
            }

            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(sql, connection);
                da.Fill(dt);
                CloseConnection(connection);
                flag = true;
                return dt;
            }
            catch (MySqlException)
            {
                flag = false;
                return dt;
            }
        }

        public MySqlConnection GetMysqlConnection()
        {
            XDocument db = XDocument.Load(@"../Resource/dbMysql.xml");
            XElement root = db.Root;

            string username = root.Element("userid").Value;
            string password = root.Element("password").Value;
            string server = root.Element("server").Value;
            string database = root.Element("database").Value;

            string connectionString = "server=" + server + ";user id=" + username + ";password=" + password + ";database=" + database;
            MySqlConnection connection = new MySqlConnection(connectionString);
            return connection;
        }

        public bool OpenConnection(MySqlConnection connection, out string message)
        {
            try
            {
                connection.Open();
                message = "接続成功です";
                return true;
            }
            catch (MySqlException ex)
            {
                message = "接続パラメートをチェックして、もう一度やり直してください";
                return false;

            }
        }

        public bool CloseConnection(MySqlConnection connection)
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }


    }
}

