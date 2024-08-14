using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Responses.NioWms
{
    /// <summary>
    /// 基础返回
    /// </summary>
    public class NioWmsBaseResponse 
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
    }

    /// <summary>
    /// NIO合作伙伴精益与库存信息
    /// </summary>
    public class NioStockInfoResponse : NioWmsBaseResponse
    {
        public List<StockPush> Data { get; set; } = new List<StockPush>();
    }

    /// <summary>
    /// 关键下级键
    /// </summary>
    public class NioKeyItemInfoResponse : NioWmsBaseResponse
    {
        public List<RawStockPush> Data { get; set; } = new List<RawStockPush>();
    }

    /// <summary>
    /// 实际交付情况推送
    /// </summary>
    public class NioWmsActualDeliveryResponse : NioWmsBaseResponse
    {
        /// <summary>
        /// 返回对象
        /// </summary>
        public List<ActualDelivery> Data { get; set; } = new List<ActualDelivery>();
    }

    /// <summary>
    /// 合作伙伴精益与库存信息
    /// </summary>
    public class StockPush
    {
        /// <summary>
        ///  合作业务
        /// </summary>
        public int PartnerBusiness { get; set; }

        /// <summary>
        ///  物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        ///  物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        ///  单位
        /// </summary>
        public string ParaConfigUnit { get; set; }

        /// <summary>
        ///  成品实际入库数量
        /// </summary>
        public decimal ProductInNum { get; set; }

        /// <summary>
        ///  成品库存合格量
        /// </summary>
        public decimal ProductStockQualified { get; set; }

        /// <summary>
        ///  成品库存不合格
        /// </summary>
        public decimal ProductStockRejection { get; set; }

        /// <summary>
        ///  成品库存待判定
        /// </summary>
        public decimal ProductStockUndetermined { get; set; }

        /// <summary>
        ///  成品备库策略（最大值）
        /// </summary>
        public decimal ProductBackUpMax { get; set; }

        /// <summary>
        ///  成品备库策略（最小值）
        /// </summary>
        public decimal ProductBackUpMin { get; set; }
    }

    /// <summary>
    /// 关键下级件信息推送
    /// </summary>
    public class RawStockPush
    {
        /// <summary>
        ///  合作业务
        /// </summary>
        public int PartnerBusiness { get; set; }

        /// <summary>
        ///  物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        ///  物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        ///  关键下级件库存合格
        /// </summary>
        public decimal SubordinateStockQualified { get; set; }

        /// <summary>
        ///  关键下级件库存不合格
        /// </summary>
        public decimal SubordinateStockRejection { get; set; }

        /// <summary>
        ///  关键下级件库存待判定
        /// </summary>
        public decimal SubordinateStockUndetermined { get; set; }

        /// <summary>
        ///  关键下级件物料编码
        /// </summary>
        public string SubordinateCode { get; set; }

        /// <summary>
        ///  关键下级件物料名称
        /// </summary>
        public string SubordinateName { get; set; }

        /// <summary>
        ///  关键下级件MOQ
        /// </summary>
        public decimal SubordinateMOQ { get; set; }

        /// <summary>
        ///  关键下级件LT
        /// </summary>
        public decimal SubordinateLT { get; set; }

        /// <summary>
        ///  关键下级件备库策略（最大值）
        /// </summary>
        public decimal SubordinateBackUpMax { get; set; }

        /// <summary>
        ///  关键下级件备库策略（最小值）
        /// </summary>
        public decimal SubordinateBackUpMin { get; set; }

        /// <summary>  
        /// 关键下级件原产国/城市  
        /// </summary>  
        public string SubordinateSource { get; set; }

        /// <summary>  
        /// 单位  
        /// </summary>  
        public string ParaConfigUnit { get; set; }
    }

    /// <summary>
    /// 实际交付情况推送
    /// </summary>
    public class ActualDelivery
    {
        /// <summary>
        ///  合作业务
        /// </summary>
        public int PartnerBusiness { get; set; }

        /// <summary>
        ///  物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        ///  物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        ///  实际发货数量
        /// </summary>
        public decimal ShippedQty { get; set; }

        /// <summary>
        ///  实际发货时间
        /// </summary>
        public string ActualDeliveryTime { get; set; }

        /// <summary>
        /// 实际发货时间
        /// </summary>
        public DateTime? ActualDeliveryDate
        {
            get
            {
                DateTime date = HymsonClock.Now();
                DateTime.TryParse(ActualDeliveryTime, out date);
                return date;
            }
        }
    }


}
