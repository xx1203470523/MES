using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 发送Http请求
    /// </summary>
    public class SendHttpDto
    {
        /// <summary>
        /// 接口地址
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// 方法 1-GET 2-POST
        /// </summary>
        public string Method { get; set; } = "1";

        /// <summary>
        /// 请求内容
        /// </summary>
        public string Body { get; set;} = string.Empty;
    }
}
