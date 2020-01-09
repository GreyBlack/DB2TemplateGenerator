using DB2TemplateGenerator.Common;
using DB2TemplateGenerator.DBHandler;
using DB2TemplateGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DB2TemplateGenerator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Generate(string table, string template, string conn, string type)
        {
            IDBHandler handler = new OracleHandler();
            List<ColumnInfo> tableColNames = handler.QueryTableColumns(conn, table);
            return Content(TemplateGenerator.Generate(table, template, tableColNames));
        }

        public IActionResult GenerateCodeFile(string table, string template, string conn, string type)
        {
            IDBHandler handler = new OracleHandler();
            List<TableInfo> tables = new List<TableInfo>();
            if (!string.IsNullOrEmpty(table))
                tables.Add(new TableInfo(table, string.Empty));
            else
                tables = handler.QueryTables(conn);
            InitTableColumns(conn, tables);
            TemplateGenerator.GenerateCodeFile(template, tables);
            return Content("");
        }

        private void InitTableColumns(string conn, List<TableInfo> tables)
        {
            foreach (var table in tables)
            {
                IDBHandler handler = new OracleHandler();
                table.TableColumns = handler.QueryTableColumns(conn, table.TableName);
            }
        }
    }
}
