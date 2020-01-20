using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace DB2TemplateGenerator.Infrastructures.Tools
{
    public class FileOperator
    {
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dirPath"></param>
        public static void CreateDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void CreateFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                FileStream fs = File.Create(filePath);
                fs.Close();
            }
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        /// <param name="path"></param>
        public static void WriteFile(string fileName, string content, string path = null)
        {
            CreateDirectory(path);
            path = path + "\\" + fileName;
            CreateFile(path);
            using (StreamWriter sw = new StreamWriter(path, true, Encoding.Default))
            {
                sw.WriteLine(content);
            }
        }

        /// <summary>
        /// 生成压缩文件
        /// </summary>
        /// <param name="fileDir"></param>
        /// <param name="zipDir"></param>
        /// <param name="zipFileName"></param>
        public static void ZipDirectoryFiles(string fileDir, string zipDir, string zipFileName)
        {
            CreateDirectory(zipDir);
            ZipFile.CreateFromDirectory(fileDir, zipDir + "/" + zipFileName);
        }

        /// <summary>
        /// 文件转化为Byte数组
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] File2Bytes(string filePath)
        {
            if (!File.Exists(filePath)) throw new NullReferenceException("文件不存在");
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                return buffer;
            }
        }

        /// <summary>
        /// 删除文件夹下所有文件
        /// </summary>
        /// <param name="dirPath"></param>
        public static void DeleteDirAllFile(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                FileInfo[] files = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (FileInfo f in files)
                {
                    File.Delete(f.FullName);
                }
            }
        }
    }
}
