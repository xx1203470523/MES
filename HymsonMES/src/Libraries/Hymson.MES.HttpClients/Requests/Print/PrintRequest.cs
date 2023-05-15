using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Requests.Print
{
    public class PrintRequest
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }
        /// <summary>
        /// 打印机名称
        /// </summary>
        public string PrinterName { get; set; }
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
}
