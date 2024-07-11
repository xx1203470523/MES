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
        /// <summary>
        /// 基础地址
        /// </summary>
        public string BaseAddressUri { get; set; } = "";
        /// <summary>
        /// 请求token
        /// </summary>
        public string SYSTOKEN { get; set; } = "2V55DBFUQOOM8OI0TE1B2ON3HF2QXK6W";
        /// <summary>
        /// 创建工单
        /// </summary>
        public string CreateOrderRoute { get; set; } = "api/Order/Create";
        /// <summary>
        /// 设置工单状态，R 进行中，T 暂停
        /// </summary>
        public string SetWorkOrderStatusRoute { get; set; }= "api/Order/SetWorkNoStatus";
        /// <summary>
        /// 物料同步
        /// </summary>
        public string MaterialSyncRoute { get; set; } = "/api/Material/Create";
    }
}
