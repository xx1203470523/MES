using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Requests.Print
{
    /// <summary>
    /// 打印请求
    /// </summary>
    public class PrintRequest
    {
        /// <summary>
        /// 打印body
        /// </summary>
        public List<PrintBody> Bodies { get; set; } = new List<PrintBody>();
    }
    public class PrintBody
    {
       
        /// <summary>
        /// 模板路径 
        /// </summary>
        public string TemplatePath { get; set; }
        /// <summary>
        /// 打印机名称
        /// </summary>
        public string PrinterName { get; set; }
        /// <summary>
        /// 打印份数
        /// </summary>
        public int PrintCount { get; set; }
        /// <summary>
        /// 传递给报表的参数集合
        /// </summary>
        public List<ParamEntity> Params { get; set; }
        /// <summary>
        /// 入参实体
        /// </summary>
        public class ParamEntity
        {
            /// <summary>
            /// 入参变量名称
            /// </summary>
            public string ParamName { get; set; }
            /// <summary>
            /// 入参变量值内容
            /// </summary>
            public string ParamValue { get; set; }

        }
    }
    public class PrintResponse
    {
        public string Data { get; set;}
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
