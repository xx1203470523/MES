using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Services.Common
{
     public  class ExcelCheck
    {

        /// <summary>
        /// 读取excel第一行的
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public  async Task<string> ReadFirstRowAsync(IFormFile formFile)
        {
            using (var package = new ExcelPackage(formFile.OpenReadStream()))
            {
                var worksheet = package.Workbook.Worksheets[0];

                // 读取第一行的数据
                var firstRowValues = new List<string>();
                for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                {
                    var cellValue = worksheet.Cells[1, col].Value?.ToString();
                    firstRowValues.Add(cellValue);
                }
                string firstRowName = string.Join("", firstRowValues);
                return firstRowName;
            }
        }

        /// <summary>
        /// 获取模板的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public  string GetColumnHeaders<T>() where T : class
        {
            Type type = typeof(T);
            var headers = new List<string>();

            PropertyInfo[] properties = type.GetProperties();

            foreach (var property in properties)
            {
                var epplusTableColumn = property.GetCustomAttribute<EpplusTableColumnAttribute>();
                if (epplusTableColumn != null)
                {
                    string header = epplusTableColumn.Header;
                    headers.Add(header);
                }
            }
            string headersName = string.Join("", headers);
            return headersName;
        }
    }
}
