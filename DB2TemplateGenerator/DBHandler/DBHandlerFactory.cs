using System;

namespace DB2TemplateGenerator.DBHandler
{
    public class DBHandlerFactory
    {
        public static IDBHandler CreateDBHandler(string type)
        {
            switch (type)
            {
                case "oracle": return new OracleHandler();
                default: throw new NullReferenceException("暂不支持指定数据库类型");
            }
        }
    }
}
