using DB2TemplateGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB2TemplateGenerator.DBHandler
{
    public interface IDBHandler
    {
        List<TableInfo> GetTableInfos(string connStr, string tableName = null);
        List<ColumnInfo> GetTableColumnInfos(string connStr, string tableName, string userName);
    }
}
