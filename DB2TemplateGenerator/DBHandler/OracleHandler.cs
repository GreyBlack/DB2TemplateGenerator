using DB2TemplateGenerator.Infrastructures.Extensions;
using DB2TemplateGenerator.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DB2TemplateGenerator.DBHandler
{
    public class OracleHandler : IDBHandler
    {


        /// <summary>
        /// 获取所有表信息（名称、注释）
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        /// <param name="tableName">指定表名称</param>
        /// <param name="userName">数据库用户名</param>
        /// <returns></returns>
        public List<TableInfo> GetTableInfos(string connStr, string tableName = null)
        {
            List<TableInfo> tables = new List<TableInfo>();
            string userName = connStr.Split("user id=")[1].Split(';')[0].ToUpper();
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                using (OracleCommand cmd = conn.CreateCommand())
                {
                    try
                    {
                        conn.Open();
                        cmd.BindByName = true;
                        string excuteSql = $"select TABLE_NAME,COMMENTS from USER_TAB_COMMENTS where 1=1";
                        excuteSql = excuteSql.ConcatIfNotEmpty(userName, $" and USER = '{userName}'");
                        excuteSql = excuteSql.ConcatIfNotEmpty(tableName, $" and TABLE_NAME = '{tableName.ToUpper()}'");
                        cmd.CommandText = excuteSql;
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            TableInfo table = new TableInfo(reader["TABLE_NAME"]?.ToString(), reader["COMMENTS"]?.ToString());
                            table.TableColumns = GetTableColumnInfos(connStr, table.TableName.ToUpper(), userName);
                            tables.Add(table);
                        }
                        reader.Dispose();
                        if (tables.Count == 0) throw new NullReferenceException("目标表不存在");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return tables;
        }

        /// <summary>
        /// 获取表内所有列信息（名称、类型、注释）
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        /// <param name="tableName">表名称</param>
        /// <param name="userName">数据库用户名</param>
        /// <returns></returns>
        public List<ColumnInfo> GetTableColumnInfos(string connStr, string tableName, string userName)
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
