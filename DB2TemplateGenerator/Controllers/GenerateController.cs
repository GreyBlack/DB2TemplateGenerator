using DB2TemplateGenerator.DBHandler;
using DB2TemplateGenerator.Generators;
using DB2TemplateGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DB2TemplateGenerator.Controllers
{
    public class GenerateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 根据模板生成映射内容
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("generate/content")]
        [HttpPost]
        public IActionResult GenerateTemplateContent([FromBody] GenerateRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.TableName)) return BadRequest(ResultMessage.Set("请输入目标表名称", false));
                IDBHandler handler = DBHandlerFactory.CreateDBHandler(request.DbType);
                List<TableInfo> tables = handler.GetTableInfos(request.Conn.ToLower(), request.TableName);
                string resContent = new BaseGenerator().GenerateContent(tables[0], request.Template);
                return Json(ResultMessage.Set("生成模板内容成功", true, data: resContent));
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ResultMessage.Set(ex.Message, false));
            }
        }

        /// <summary>
        /// 根据模板生成文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("generate/file")]
        [HttpPost]
        public IActionResult GenerateFile([FromBody] GenerateRequest request)
        {
            try
            {
                IDBHandler handler = DBHandlerFactory.CreateDBHandler(request.DbType);
                List<TableInfo> tables = handler.GetTableInfos(request.Conn.ToLower(), request.TableName);
                IFileGenerator generator = GeneratorFactory.NewGenerator(request.FileType);
                GenerateFileInfo fileInfo = null;
                if (tables.Count == 1)
                    fileInfo = generator.GenerateFile(tables[0], request.Template);
                else if (tables.Count > 1)
                    fileInfo = generator.GenerateZip(tables, request.Template, request.FileType);
                if (fileInfo == null) return BadRequest(ResultMessage.Set("文件不存在", false));
                return File(fileInfo.FileBytes, fileInfo.MimeType, fileInfo.FileName);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ResultMessage.Set(ex.Message, false));
            }
        }
    }
}
