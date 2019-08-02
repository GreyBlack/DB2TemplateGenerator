using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DB2TemplateGenerator.Common
{
    public class TemplateGenerator
    {
        public static string Generate(string tableName, string template, List<string> tableColNames, List<string> propertyNames)
        {
            string templateRes = string.Empty;
            string className = tableName.ToEntityName();
            // 替换表明
            template = template.Replace("[table]", tableName);
            // 替换类名
            template = template.Replace("[class]", className);
            // 检测循环 TODO 正则匹配循环内字符串  %->问题待处理
            Regex regex = new Regex(@"<%[\d\D]*?%>");
            StringBuilder sb = new StringBuilder();
            template = regex.Replace(template, (Match m) =>
            {
                string matchStr = string.Empty;
                for (int i = 0; i < tableColNames.Count; i++)
                {
                    matchStr = m.ToString();
                    matchStr = matchStr.Replace("[column]", tableColNames[i]);
                    matchStr = matchStr.Replace("[property]", propertyNames[i]);
                    if (i == tableColNames.Count - 1)
                    {
                        int lastComma = matchStr.LastIndexOf(',');
                        if (lastComma > 0) sb.Append(matchStr.Substring(0, matchStr.LastIndexOf(',')));
                        else sb.Append(matchStr);
                    }
                    else
                        sb.AppendLine(matchStr);
                }
                string replaceStr = sb.ToString();
                sb.Clear();
                return replaceStr;
            });
            templateRes = new Regex(@"<%|%>").Replace(template, "");
            return templateRes;
        }
    }
}
