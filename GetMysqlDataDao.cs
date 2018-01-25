using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL.Dao
{
    interface GetMysqlDataDao
    {
        DataTable GetDataTableFromMysql(string sql, MySqlConnection connection, out bool flag);

        MySqlConnection GetMysqlConnection();

        bool OpenConnection(MySqlConnection connection, out string message);

        bool CloseConnection(MySqlConnection connection);
    }
}
