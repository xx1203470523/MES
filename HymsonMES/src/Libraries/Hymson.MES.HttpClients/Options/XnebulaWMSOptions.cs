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
    public class XnebulaWMSOption
    {
        /// <summary>
        /// 基础路径
        /// </summary>
        public string BaseAddressUri { get; set; } = "http://192.168.126.148:20040/";
        public string Token { get; set; } = "";
        /// <summary>
        /// 出库操作配置
        /// </summary>
        public WMSDeliveryOptions Delivery { get; set; }
        /// <summary>
        /// 取消出库操作配置
        /// </summary>
        public WMSDeliveryCancelOptions DeliveryCancel { get; set; }
        /// <summary>
        /// 入库操作配置
        /// </summary>
        public WMSReceiptOptions Receipt { get; set; }
        /// <summary>
        /// 取消入库操作配置
        /// </summary>
        public WMSReceiptCancelOptions ReceiptCancel { get; set; }

        /// <summary>
        /// 成品入库单配置 
        /// </summary>
        public WMSProductReceiptOptions ProductReceipt { get; set; }

        /// <summary>
        /// 取消成品入库操作配置
        /// </summary>
        public WMSProductReceiptCancelOptions ProductReceiptCancel { get; set; }
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

    public class WMSProductReceiptOptions
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

    public class WMSProductReceiptCancelOptions
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
