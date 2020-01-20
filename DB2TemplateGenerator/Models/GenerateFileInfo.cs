using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB2TemplateGenerator.Models
{
    public class GenerateFileInfo
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string MimeType { get; set; }
        /// <summary>
        /// 文件Bytes
        /// </summary>
        public byte[] FileBytes { get; set; }

        public GenerateFileInfo(string fileName, string mimeType, byte[] fileBytes)
        {
            FileName = fileName;
            MimeType = mimeType;
            FileBytes = fileBytes;
        }
    }
}
