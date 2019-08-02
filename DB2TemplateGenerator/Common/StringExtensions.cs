using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB2TemplateGenerator.Common
{
    public static class StringExtensions
    {
        public static string ToEntityName(this string tableName)
        {
            string entityStr = string.Empty;
            if (!string.IsNullOrEmpty(tableName))
            {
                var strArr = tableName.Split("_");
                foreach (var str in strArr)
                {
                    string pascalInital = str.Substring(0, 1).ToUpper();
                    string pascalRest = str.Substring(1).ToLower();
                    entityStr += pascalInital + pascalRest;
                }
            }
            return entityStr;
        }

        public static string ToPropertyName(this string colName)
        {
            string entityStr = string.Empty;
            if (!string.IsNullOrEmpty(colName))
            {
                char initStr = colName[0];
                colName = colName.Substring(1);
                var strArr = colName.Split("_");
                foreach (var str in strArr)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        string pascalInital = str.Substring(0, 1).ToUpper();
                        string pascalRest = str.Substring(1).ToLower();
                        entityStr += pascalInital + pascalRest;
                    }
                }
                entityStr = initStr + entityStr;
            }
            return entityStr;
        }
    }
}
