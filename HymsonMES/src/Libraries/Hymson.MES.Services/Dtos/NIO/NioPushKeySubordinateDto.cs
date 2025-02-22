using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Plan;

namespace Hymson.MES.Services.Dtos.NIO
{
    /// <summary>
    /// 物料及其关键下级件信息表新增/更新Dto
    /// </summary>
    public record NioPushKeySubordinateSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

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

       /// <summary>
        /// 
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 物料及其关键下级件信息表Dto
    /// </summary>
    public record NioPushKeySubordinateDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

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

       /// <summary>
        /// 
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 推送状态;0：无需推送；1：待推送；2：已推送；3：推送失败；
        /// </summary>
        public PushStatusEnum Status { get; set; }
    }

    /// <summary>
    /// 物料及其关键下级件信息表分页Dto
    /// </summary>
    public class NioPushKeySubordinatePagedQueryDto : PagerInfo 
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 推送状态;0：无需推送；1：待推送；2：已推送；3：推送失败；
        /// </summary>
        public PushStatusEnum? Status { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string ?Date { get; set; }

        /// <summary>
        /// 关键下级件物料编码
        /// </summary>
        public string ?SubordinateCode { get; set; }

        /// <summary>
        /// 关键下级件物料名称
        /// </summary>
        public string ?SubordinateName { get; set; }
    }

}
