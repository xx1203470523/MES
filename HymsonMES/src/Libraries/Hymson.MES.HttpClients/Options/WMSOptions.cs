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
        /// 出库操作配置
        /// </summary>
        public WMSDeliveryOptions DeliveryOptions { get; set; }
        public WMSDeliveryCancelOptions DeliveryCancelOptions { get; set; }
        /// <summary>
        /// 入库操作配置
        /// </summary>
        public WMSReceiptOptions ReceiptOptions { get; set; }
        public WMSReceiptCancelOptions ReceiptCancelOptions { get; set; }
    }
    /// <summary>
    /// 出库单操作
    /// </summary>
    public class WMSDeliveryOptions
    {
       
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// 单据类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 路由信息
        /// </summary>
        public string RoutePath { get; set; }
    }
    public class WMSDeliveryCancelOptions
    {

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// 单据类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 路由信息
        /// </summary>
        public string RoutePath { get; set; }
    }
    /// <summary>
    /// 入库单操作
    /// </summary>
    public class WMSReceiptOptions
    {
       
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// 单据类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 路由信息
        /// </summary>
        public string RoutePath { get; set; }
    }
    public class WMSReceiptCancelOptions
    {

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// 单据类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 路由信息
        /// </summary>
        public string RoutePath { get; set; }
    }
}
