using DB2TemplateGenerator.Models;
using System.Collections.Generic;

namespace DB2TemplateGenerator.Generators
{
    /// <summary>
    /// 生成字符串
    /// </summary>
    public class XmlGenerator : BaseGenerator, IFileGenerator
    {

        public GenerateFileInfo GenerateFile(TableInfo table, string template)
        {
            return base.GenerateFile(table, template, "application/xml", ".xml");
        }

        public GenerateFileInfo GenerateZip(List<TableInfo> tables, string template)
        {
            return base.GenerateZip(tables, template, "mappers.zip");
        }
    }
}
