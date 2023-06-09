using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Requests.Print
{
    /// <summary>
    /// 打印预览请求
    /// </summary>
    public class PreviewRequest
    {
        /// <summary>
        /// 模板路径
        /// </summary>
        public string TemplatePath { get; set; }
        /// <summary>
        /// 传递给报表的参数集合
        /// </summary>
        public List<PrintBody.ParamEntity> Params { get; set; }
    }
}
