using DB2TemplateGenerator.Models;
using System.Collections.Generic;

namespace DB2TemplateGenerator.Generators
{
    /// <summary>
    /// 生成字符串
    /// </summary>
    public class SqlGenerator : BaseGenerator, IFileGenerator
    {

        public GenerateFileInfo GenerateFile(TableInfo table, string template)
        {
            return base.GenerateFile(table, template, "application/octet-stream", ".sql");
        }

        public GenerateFileInfo GenerateZip(List<TableInfo> tables, string template, string fileType)
        {
            return base.GenerateZip(tables, template, fileType, "sqls.zip");
        }
    }
}
