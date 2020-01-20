namespace DB2TemplateGenerator.Infrastructures.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 如果条件字符串不为空，拼接后续字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conditionStr">前置条件字符串</param>
        /// <param name="nextStr">后续拼接字符串</param>
        /// <returns></returns>
        public static string ConcatIfNotEmpty(this string value, string conditionStr, string nextStr)
        {
            return string.IsNullOrEmpty(conditionStr) ? value : (value + nextStr);
        }

        /// <summary>
        /// Oracle类型映射C#类型
        /// </summary>
        /// <param name="oracleType">Oracle类型</param>
        /// <returns></returns>
        public static string OracleToNormalType(this string oracleType)
        {
            switch (oracleType)
            {
                case "NUMBER": return "long";
                case "DATE": return "DateTime";
                default: return "string";
            }
        }

        /// <summary>
        /// 表名转换为实体名 大写+下划线->Pascal
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static string ToClassName(this string tableName)
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

        /// <summary>
        /// 列名转换为属性名 大写+下划线->去除首位字母+Pascal
        /// </summary>
        /// <param name="colName">列名</param>
        /// <returns></returns>
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
