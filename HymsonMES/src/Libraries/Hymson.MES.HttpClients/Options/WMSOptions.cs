using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Options
{
    /// <summary>
    /// wms操作配置
    /// </summary>
    public class WMSOptions
    {
        /// <summary>
        /// 基础地址
        /// </summary>
        public string BaseAddressUri { get; set; } = "http://192.168.126.148:20040/";

        /// <summary>
        /// 请求token
        /// 2V55DBFUQOOM8OI0TE1B2ON3HF2QXK6W
        /// </summary>
        public string SYSTOKEN { get; set; } = "2V55DBFUQOOM8OI0TE1B2ON3HF2QXK6W";

        /// <summary>
        /// 创建工单
        /// </summary>
        public string CreateOrderRoute { get; set; } = "api/Order/Create";
    }
}
