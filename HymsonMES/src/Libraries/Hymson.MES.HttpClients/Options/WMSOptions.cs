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
        public string BaseAddressUri { get; set; } = "";

        /// <summary>
        /// 请求token
        /// 2V55DBFUQOOM8OI0TE1B2ON3HF2QXK6W
        /// </summary>
        public string SysToken { get; set; } = "";

        /// <summary>
        /// 创建工单
        /// </summary>
        public string CreateOrderRoute { get; set; } = "api/Order/Create";
    }
}
