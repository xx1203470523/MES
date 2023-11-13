using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Common
{
    public class ExportResultDto
    {
        /// <summary>
        /// 绝对地址
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// 相对地址
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
    }
}
