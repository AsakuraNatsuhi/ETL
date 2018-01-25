using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL.Dao
{
    interface GetOracleDataDao
    {
        OracleConnection GetOracleConnection();

        DataTable GetDataTableFromOracle(string sql, OracleConnection connection, out bool flag);

        bool OpenConnection(OracleConnection connection, out string message);

        bool CloseConnection(OracleConnection connection);


    }
}
