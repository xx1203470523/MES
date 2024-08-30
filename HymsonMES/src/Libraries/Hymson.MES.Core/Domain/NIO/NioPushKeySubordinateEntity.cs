using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.NIO
{
    /// <summary>
    /// 数据实体（物料及其关键下级件信息表）   
    /// nio_push_keySubordinate
    /// @author User
    /// @date 2024-08-30 03:48:41
    /// </summary>
    public class NioPushKeySubordinateEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long NioPushId { get; set; }

        /// <summary>
        /// 合作业务（1:电池，2:电驱）
        /// </summary>
        public int PartnerBusiness { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 日期（格式为yyyy-MM-dd HH:mm:ss）
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 关键下级件物料编码
        /// </summary>
        public string SubordinateCode { get; set; }

        /// <summary>
        /// 关键下级件物料名称
        /// </summary>
        public string SubordinateName { get; set; }

        /// <summary>
        /// 关键下级件MOQ（最小订单量）
        /// </summary>
        public decimal SubordinateMOQ { get; set; }

        /// <summary>
        /// 关键下级件LT（前置时间/交货期）
        /// </summary>
        public decimal SubordinateLT { get; set; }

        /// <summary>
        /// 关键下级件台套用量
        /// </summary>
        public decimal SubordinateDosage { get; set; }

        /// <summary>
        /// 关键下级件合作方
        /// </summary>
        public string SubordinatePartner { get; set; }

        /// <summary>
        /// 关键下级件原产国/城市
        /// </summary>
        public string SubordinateSource { get; set; }

        /// <summary>
        /// 关键下级件备库策略（最大值）
        /// </summary>
        public decimal SubordinateBackUpMax { get; set; }

        /// <summary>
        /// 关键下级件备库策略（最小值）
        /// </summary>
        public decimal SubordinateBackUpMin { get; set; }

        /// <summary>
        /// 关键下级件库存合格量
        /// </summary>
        public decimal SubordinateStockQualified { get; set; }

        /// <summary>
        /// 关键下级件库存不合格量
        /// </summary>
        public decimal SubordinateStockRejection { get; set; }

        /// <summary>
        /// 关键下级件库存待判定量
        /// </summary>
        public decimal SubordinateStockUndetermined { get; set; }

        /// <summary>
        /// 关键下级件到货计划
        /// </summary>
        public decimal SubordinateArrivalPlan { get; set; }

        /// <summary>
        /// 关键下级件需求计划
        /// </summary>
        public decimal SubordinateDemandPlan { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string ParaConfigUnit { get; set; }

        
    }
}
