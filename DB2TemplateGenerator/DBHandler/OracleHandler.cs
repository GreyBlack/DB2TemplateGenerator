using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB2TemplateGenerator.DBHandler
{
    public class OracleHandler : IDBHandler
    {
        public List<string> QueryTableCollection(string connStr)
        {
            throw new NotImplementedException();
        }

        public List<string> QueryTableColums(string connStr, string tableName)
        {
            List<string> cols = new List<string>();
            using (OracleConnection con = new OracleConnection(connStr))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        cmd.CommandText = string.Format("SELECT T.COLUMN_NAME FROM USER_TAB_COLUMNS T WHERE T.TABLE_NAME='{0}' ORDER BY T.COLUMN_ID", tableName);
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            cols.Add(reader.GetString(0));
                        }
                        reader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return cols;
        }
    }
}
