using DB2TemplateGenerator.Models;
using System.Collections.Generic;

namespace DB2TemplateGenerator.Generators
{
    public class CSharpGenerator : BaseGenerator, IFileGenerator
    {
        public GenerateFileInfo GenerateFile(TableInfo table, string template)
        {
            return base.GenerateFile(table, template, "application/octet-stream", ".cs");
        }

        public GenerateFileInfo GenerateZip(List<TableInfo> tables, string template, string fileType)
        {
            return base.GenerateZip(tables, template, fileType, "classes.zip");
        }
    }
}
