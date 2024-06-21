using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients
{
    /// <summary>
    /// 仓库操作
    /// </summary>
    public class WMSOptions
    {
        /// <summary>
        /// 基础路径
        /// </summary>
        public string BaseAddressUri { get; set; } = "";
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// 领料单申请相对路径
        /// </summary>
        public string MaterialPickingRequestUrl { get; set; }
    }
}
