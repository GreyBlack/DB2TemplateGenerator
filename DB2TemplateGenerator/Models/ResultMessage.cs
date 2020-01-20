namespace DB2TemplateGenerator.Models
{
    public class ResultMessage
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }

        public ResultMessage(bool success, string message, dynamic data, string code)
        {
            Success = success;
            Message = string.IsNullOrEmpty(message) ? success ? "操作成功" : "操作失败" : message;
            Data = data;
            Code = code;
        }

        public static ResultMessage Set(string message, bool success, string code = "")
        {
            return new ResultMessage(success, message, null, code);
        }

        public static ResultMessage Set<T>(string message, bool success, T data = default(T), string code = "")
        {
            return new ResultMessage(success, message, data, code);
        }
    }
}
