using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DB2TemplateGenerator.Models;
using DB2TemplateGenerator.DBHandler;
using DB2TemplateGenerator.Common;

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
            List<string> tableColNames = handler.QueryTableColums(conn, table);
            List<string> propertyNames = tableColNames.Select(c => c.ToPropertyName()).ToList();
            return Content(TemplateGenerator.Generate(table, template, tableColNames, propertyNames));
        }
    }
}
