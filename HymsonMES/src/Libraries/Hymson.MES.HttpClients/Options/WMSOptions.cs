namespace Hymson.MES.HttpClients.Options
{
    /// <summary>
    /// WMS操作配置
    /// </summary>
    public class WMSOptions
    {
        /// <summary>
        /// 基础地址
        /// </summary>
        public string BaseAddressUri { get; set; } = "";

        /// <summary>
        /// 请求token
        /// </summary>
        public string SysToken { get; set; } = "";

        /// <summary>
        /// WMS回调路径（来料）
        /// </summary>
        public string IQCReceiptRoute { get; set; } = "";

        /// <summary>
        /// WMS回调路径（退料）
        /// </summary>
        public string IQCReturnRoute { get; set; } = "";

        /// <summary>
        /// 出库操作配置
        /// </summary>
        public WMSBusinessOptions Delivery { get; set; }

        /// <summary>
        /// 取消出库操作配置
        /// </summary>
        public WMSBusinessOptions DeliveryCancel { get; set; }

        /// <summary>
        /// 入库操作配置
        /// </summary>
        public WMSBusinessOptions Receipt { get; set; }

        /// <summary>
        /// 取消入库操作配置
        /// </summary>
        public WMSBusinessOptions ReceiptCancel { get; set; }

        /// <summary>
        /// 成品入库单配置 
        /// </summary>
        public WMSBusinessOptions ProductReceipt { get; set; }

        /// <summary>
        /// 取消成品入库操作配置
        /// </summary>
        public WMSBusinessOptions ProductReceiptCancel { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class WMSBusinessOptions
    {
        /// <summary>
        /// 单据类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; } = "";

        /// <summary>
        /// 路由信息
        /// </summary>
        public string Route { get; set; } = "";

        /// <summary>
        /// 原材料仓库
        /// </summary>
        public string RawWarehouseCode { get; set; }

        /// <summary>
        /// 虚拟仓库
        /// </summary>
        public string VirtuallyWarehouseCode { get; set; }

        /// <summary>
        /// 待检线边仓
        /// </summary>
        public string PendInspection { get; set; }
    }

}
