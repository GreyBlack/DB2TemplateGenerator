using DB2TemplateGenerator.Models;
using System.Collections.Generic;

namespace DB2TemplateGenerator.Generators
{
    /// <summary>
    /// 文件生成器接口
    /// </summary>
    public interface IFileGenerator
    {
        GenerateFileInfo GenerateFile(TableInfo table, string template);
        GenerateFileInfo GenerateZip(List<TableInfo> tables, string template);
    }
}
