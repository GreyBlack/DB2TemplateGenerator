using DB2TemplateGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DB2TemplateGenerator.Common
{
    public class TemplateGenerator
    {
        public static string Generate(string tableName, string template, List<ColumnInfo> tableColNames)
        {
            string templateRes = string.Empty;
            // 替换表名
            template = template.Replace("[table]", tableName);
            // 替换类名
            template = template.Replace("[class]", tableName.ToEntityName());
            // 检测循环 TODO 正则匹配循环内字符串  %->问题待处理
            Regex regex = new Regex(@"<%[\d\D]*?%>");
            StringBuilder sb = new StringBuilder();
            List<string> propertyNames = tableColNames.Select(c => c.ColumnName.ToPropertyName()).ToList();
            template = regex.Replace(template, (Match m) =>
            {
                string matchStr = string.Empty;
                for (int i = 0; i < tableColNames.Count; i++)
                {
                    matchStr = m.ToString();
                    matchStr = matchStr.Replace("[column]", tableColNames[i].ColumnName);
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

        public static void GenerateCodeFile(string template, List<TableInfo> tables)
        {
            foreach (var table in tables)
            {
                string templateRes = template;
                // 表注释
                templateRes = templateRes.Replace("[table-comment]", table.TableComment);
                // 替换表名
                templateRes = templateRes.Replace("[table]", table.TableName);
                // 检测循环 TODO 正则匹配循环内字符串  %->问题待处理
                Regex regex = new Regex(@"<%[\d\D]*?%>");
                StringBuilder sb = new StringBuilder();
                templateRes = regex.Replace(templateRes, (Match m) =>
                {
                    string matchStr = string.Empty;
                    for (int i = 0; i < table.TableColumns.Count; i++)
                    {
                        matchStr = m.ToString();
                        if (i == 0)
                        {
                            matchStr = matchStr.Replace("public", "[Key]\r\n        public");
                        }
                        matchStr = matchStr.Replace("[column]", table.TableColumns[i].ColumnName);
                        matchStr = matchStr.Replace("[column-type]", table.TableColumns[i].ColumnType.OracleToNormalType());
                        matchStr = matchStr.Replace("[column-comment]", table.TableColumns[i].ColumnComment);
                        sb.AppendLine(matchStr);
                    }
                    string replaceStr = sb.ToString();
                    sb.Clear();
                    return replaceStr;
                });
                templateRes = new Regex(@"<%|%>").Replace(templateRes, "").Replace("\n", "\r\n").Replace("\r\r\n", "\r\n");
                WriteFile(table.TableName, templateRes);
            }
        }

        private static void WriteFile(string fileName, string content, string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "/App_Data";
            }
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = path + "\\" + fileName + ".cs";
            if (!File.Exists(path))
            {
                FileStream fs = File.Create(path);
                fs.Close();
            }
            StreamWriter sw = new StreamWriter(path, true, Encoding.Default);
            sw.WriteLine(content);
            sw.Close();
        }
    }
}
