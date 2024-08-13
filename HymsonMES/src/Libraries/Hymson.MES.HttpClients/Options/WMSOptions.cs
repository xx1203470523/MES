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
        public WMSBusinessOptions IQCReceipt { get; set; }

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

        /// <summary>
        /// NIO合作伙伴精益与库存信息
        /// </summary>
        public WMSBusinessOptions NioStockInfo { get; set; }

        /// <summary>
        /// 关键下级键
        /// </summary>
        public WMSBusinessOptions NioKeyItemInfo { get; set; }

        /// <summary>
        /// 实际交付情况
        /// </summary>
        public WMSBusinessOptions NioActualDelivery { get; set; }
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
        /// 默认仓库
        /// </summary>
        public string WarehouseCode { get; set; } = "";

        /// <summary>
        /// 路由信息
        /// </summary>
        public string Route { get; set; } = "";

        /// <summary>
        /// 原材料仓库
        /// </summary>
        public string RawWarehouseCode { get; set; } = "";

        /// <summary>
        /// 虚拟仓库
        /// </summary>
        public string VirtuallyWarehouseCode { get; set; } = "";

        /// <summary>
        /// 待检线边仓
        /// </summary>
        public string PendInspection { get; set; } = "";

        /// <summary>
        /// 成品仓
        /// </summary>
        public string FinishWarehouseCode { get; set; } = "";

        /// <summary>
        /// 不良品仓
        /// </summary>
        public string NgWarehouseCode { get; set; } = "";

    }

}
