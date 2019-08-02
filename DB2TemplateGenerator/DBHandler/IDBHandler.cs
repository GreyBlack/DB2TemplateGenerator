using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB2TemplateGenerator.DBHandler
{
    public interface IDBHandler
    {
        List<string> QueryTableCollection(string connStr);
        List<string> QueryTableColums(string connStr, string tableName);
    }
}
