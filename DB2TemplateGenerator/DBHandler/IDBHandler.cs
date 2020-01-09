using DB2TemplateGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB2TemplateGenerator.DBHandler
{
    public interface IDBHandler
    {
        List<TableInfo> QueryTables(string connStr, string userName = "MEDICALUSER");
        List<ColumnInfo> QueryTableColumns(string connStr, string tableName, string userName = "MEDICALUSER");
    }
}
