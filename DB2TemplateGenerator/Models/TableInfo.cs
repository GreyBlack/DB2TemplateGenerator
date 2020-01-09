using System.Collections.Generic;

namespace DB2TemplateGenerator.Models
{
    public class TableInfo
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表注释
        /// </summary>
        public string TableComment { get; set; }
        /// <summary>
        /// 表内字段
        /// </summary>
        public List<ColumnInfo> TableColumns { get; set; }

        public TableInfo(string tableName, string tableComment)
        {
            TableName = tableName;
            TableComment = tableComment;
            TableColumns = new List<ColumnInfo>();
        }
    }
}
