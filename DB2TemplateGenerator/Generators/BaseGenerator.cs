using DB2TemplateGenerator.Infrastructures.Extensions;
using DB2TemplateGenerator.Infrastructures.Tools;
using DB2TemplateGenerator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DB2TemplateGenerator.Generators
{
    public class BaseGenerator
    {
        public static string TEMP_FILE_PATH = AppDomain.CurrentDomain.BaseDirectory + "/files";
        public static string TEMP_ZIP_PATH = AppDomain.CurrentDomain.BaseDirectory + "/zips";

        /// <summary>
        /// 生成模板内容
        /// </summary>
        /// <param name="table">表信息</param>
        /// <param name="template">模板字符串</param>
        /// <returns></returns>
        public string GenerateContent(TableInfo table, string template)
        {
            // 检测循环 TODO 正则匹配循环内字符串  %->问题待处理
            Regex regex = new Regex(@"<%[\d\D]*?%>");
            StringBuilder sb = new StringBuilder();
            string resultContent = template;
            // 替换表名
            resultContent = resultContent.Replace("[table]", table.TableName);
            // 替换表注释
            resultContent = resultContent.Replace("[table-comment]", table.TableComment);
            // 替换类名
            resultContent = resultContent.Replace("[class]", table.TableName.ToClassName());
            resultContent = regex.Replace(resultContent, (Match m) =>
            {
                string matchStr = string.Empty;
                ColumnInfo column = null;
                for (int i = 0; i < table.TableColumns.Count; i++)
                {
                    matchStr = m.ToString();
                    //if (i == 0)
                    //{
                    //    matchStr = matchStr.Replace("public", "[Key]\r\n        public");
                    //}
                    column = table.TableColumns[i];
                    // 字段映射
                    matchStr = matchStr.Replace("[column]", column.ColumnName);
                    matchStr = matchStr.Replace("[column-type]", column.ColumnType);
                    matchStr = matchStr.Replace("[column-comment]", column.ColumnComment);
                    // 属性映射
                    matchStr = matchStr.Replace("[property]", column.ColumnName.ToPropertyName());
                    matchStr = matchStr.Replace("[property-type]", column.ColumnType.OracleToNormalType());
                    // 如果掉循环字段包含逗号 去除最后一个 
                    if (i == table.TableColumns.Count - 1)
                    {
                        int lastComma = matchStr.LastIndexOf(',');
                        if (lastComma > 0) sb.Append(matchStr.Remove(lastComma, 1));
                    }
                    else
                        sb.AppendLine(matchStr);
                }
                string replaceStr = sb.ToString();
                sb.Clear();
                return replaceStr;
            });
            return new Regex(@"<%|%>").Replace(resultContent, "").Replace("[key]", table.TableColumns[0].ColumnName.ToPropertyName()).Replace("\n", "\r\n").Replace("\r\r\n", "\r\n");
        }

        /// <summary>
        /// 生成文件
        /// </summary>
        /// <param name="tables">表信息</param>
        /// <param name="template">模板字符串</param>
        /// <param name="contentType">文件类型</param>
        /// <param name="suffix">文件后缀</param>
        /// <returns></returns>
        public virtual GenerateFileInfo GenerateFile(TableInfo table, string template, string contentType, string suffix)
        {

            string resultContent = GenerateContent(table, template);
            string fileName = table.TableName.ToClassName() + suffix;
            FileOperator.DeleteDirAllFile(TEMP_FILE_PATH);
            FileOperator.WriteFile(fileName, resultContent, TEMP_FILE_PATH);
            return new GenerateFileInfo(fileName, contentType, FileOperator.File2Bytes(TEMP_FILE_PATH + "/" + fileName));
        }

        /// <summary>
        /// 生成zip
        /// </summary>
        /// <param name="tables">表信息</param>
        /// <param name="template">模板字符串</param>
        /// <param name="contentType">文件类型</param>
        /// <param name="zipName">zip名称</param>
        /// <returns></returns>
        public virtual GenerateFileInfo GenerateZip(List<TableInfo> tables, string template, string zipName)
        {
            FileOperator.DeleteDirAllFile(TEMP_FILE_PATH);
            FileOperator.DeleteDirAllFile(TEMP_ZIP_PATH);
            foreach (var table in tables)
            {
                string resultContent = GenerateContent(table, template);
                string fileName = table.TableName.ToClassName() + ".cs";
                FileOperator.WriteFile(fileName, resultContent, TEMP_FILE_PATH);
            }
            FileOperator.ZipDirectoryFiles(TEMP_FILE_PATH, TEMP_ZIP_PATH, zipName);
            return new GenerateFileInfo(zipName, "application/zip", FileOperator.File2Bytes(TEMP_ZIP_PATH + "/" + zipName));
        }
    }
}
