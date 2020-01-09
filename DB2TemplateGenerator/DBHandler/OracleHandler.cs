using DB2TemplateGenerator.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DB2TemplateGenerator.DBHandler
{
    public class OracleHandler : IDBHandler
    {
        public List<string> QueryTableCollection(string connStr)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取所有表名称
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<TableInfo> QueryTables(string connStr, string userName = "MEDICALUSER")
        {
            List<TableInfo> tables = new List<TableInfo>();
            using (OracleConnection con = new OracleConnection(connStr))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        cmd.CommandText = $"select TABLE_NAME,COMMENTS from USER_TAB_COMMENTS where USER = '{userName}'";
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            tables.Add(new TableInfo(reader["TABLE_NAME"]?.ToString(), reader["COMMENTS"]?.ToString()));
                        }
                        reader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return tables;
        }

        public List<ColumnInfo> QueryTableColumns(string connStr, string tableName, string userName = "MEDICALUSER")
        {
            List<ColumnInfo> cols = new List<ColumnInfo>();
            using (OracleConnection con = new OracleConnection(connStr))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        cmd.CommandText = $"select distinct A.COLUMN_NAME,B.DATA_TYPE,A.COMMENTS,B.COLUMN_ID from USER_COL_COMMENTS A left join USER_TAB_COLUMNS B on A.TABLE_NAME = B.TABLE_NAME and A.COLUMN_NAME = B.COLUMN_NAME" +
                            $" where USER = '{userName}' and A.TABLE_NAME = '{tableName}' order by B.COLUMN_ID";
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            cols.Add(new ColumnInfo(reader["COLUMN_NAME"]?.ToString(), reader["DATA_TYPE"]?.ToString(), reader["COMMENTS"]?.ToString()));
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
