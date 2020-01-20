namespace DB2TemplateGenerator.Models
{
    public class GenerateRequest
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Conn { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 对应下载文件类型
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// 使用模板字符串
        /// </summary>
        public string Template { get; set; }
    }
}
