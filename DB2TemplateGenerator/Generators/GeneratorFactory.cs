
using System;

namespace DB2TemplateGenerator.Generators
{
    public class GeneratorFactory
    {
        public static IFileGenerator NewGenerator(string fileType)
        {
            switch (fileType)
            {
                case "sql": return new SqlGenerator();
                case "xml": return new XmlGenerator();
                case "cs": return new CSharpGenerator();
                default: throw new NullReferenceException("暂无指定文件类型生成器");
            }
        }
    }
}
