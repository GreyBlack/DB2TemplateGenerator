namespace DB2TemplateGenerator.Models
{
    public class ColumnInfo
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string ColumnType { get; set; }
        /// <summary>
        /// 字段注释
        /// </summary>
        public string ColumnComment { get; set; }

        public ColumnInfo(string columnName, string columnType, string columnComment)
        {
            ColumnName = columnName;
            ColumnType = columnType;
            ColumnComment = columnComment;
        }
    }
}
