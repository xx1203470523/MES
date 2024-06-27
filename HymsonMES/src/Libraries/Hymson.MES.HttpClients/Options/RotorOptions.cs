using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Options
{
    /// <summary>
    /// 转子操作配置
    /// </summary>
    public class RotorOption
    {
        public string BaseAddressUri { get; set; } = "";
        /// <summary>
        /// 请求token
        /// </summary>
        public string SYSTOKEN { get; set; } = "2V55DBFUQOOM8OI0TE1B2ON3HF2QXK6W";
        /// <summary>
        /// 创建工单
        /// </summary>
        public string CreateOrderRoute { get; set; } = "api/Order/Create";
        public string StartOrderRoute { get; set; }= "api/Order/Start?id=";
        public string StopOrderRoute { get; set; } = "api/Order/Stop?id=";
    }
}
