using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Plan;

namespace Hymson.MES.Services.Dtos.NIO
{
    /// <summary>
    /// 合作伙伴精益与生产能力新增/更新Dto
    /// </summary>
    public record NioPushProductioncapacitySaveDto : BaseEntityDto
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
        /// 排班数/天
        /// </summary>
        public int WorkingSchedule { get; set; }

        /// <summary>
        /// 计划产能/天
        /// </summary>
        public decimal PlannedCapacity { get; set; }

        /// <summary>
        /// 稼动率（%）
        /// </summary>
        public decimal Efficiency { get; set; }

        /// <summary>
        /// 节拍（s）
        /// </summary>
        public decimal Beat { get; set; }

        /// <summary>
        /// 日生产工单或生产计划
        /// </summary>
        public decimal DailyProductionPlan { get; set; }

       /// <summary>
        /// 瓶颈工序
        /// </summary>
        public string BottleneckProcess { get; set; }

        /// <summary>
        /// 下线合格数量
        /// </summary>
        public decimal DownlineNum { get; set; }

        /// <summary>
        /// 成品实际入库数量
        /// </summary>
        public decimal ProductInNum { get; set; }

        /// <summary>
        /// 成品库存合格量
        /// </summary>
        public decimal ProductStockQualified { get; set; }

        /// <summary>
        /// 成品库存不合格量
        /// </summary>
        public decimal ProductStockRejection { get; set; }

        /// <summary>
        /// 成品库存待判定
        /// </summary>
        public decimal ProductStockUndetermined { get; set; }

        /// <summary>
        /// 成品备库策略（最大值）
        /// </summary>
        public decimal ProductBackUpMax { get; set; }

        /// <summary>
        /// 成品备库策略（最小值）
        /// </summary>
        public decimal ProductBackUpMin { get; set; }

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
    /// 合作伙伴精益与生产能力Dto
    /// </summary>
    public record NioPushProductioncapacityDto : BaseEntityDto
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
        /// 排班数/天
        /// </summary>
        public int WorkingSchedule { get; set; }

        /// <summary>
        /// 计划产能/天
        /// </summary>
        public decimal PlannedCapacity { get; set; }

       /// <summary>
        /// 稼动率（%）
        /// </summary>
        public decimal Efficiency { get; set; }

       /// <summary>
        /// 节拍（s）
        /// </summary>
        public decimal Beat { get; set; }

       /// <summary>
        /// 日生产工单或生产计划
        /// </summary>
        public decimal DailyProductionPlan { get; set; }

       /// <summary>
        /// 瓶颈工序
        /// </summary>
        public string BottleneckProcess { get; set; }

       /// <summary>
        /// 下线合格数量
        /// </summary>
        public decimal DownlineNum { get; set; }

       /// <summary>
        /// 成品实际入库数量
        /// </summary>
        public decimal ProductInNum { get; set; }

       /// <summary>
        /// 成品库存合格量
        /// </summary>
        public decimal ProductStockQualified { get; set; }

       /// <summary>
        /// 成品库存不合格量
        /// </summary>
        public decimal ProductStockRejection { get; set; }

       /// <summary>
        /// 成品库存待判定
        /// </summary>
        public decimal ProductStockUndetermined { get; set; }

       /// <summary>
        /// 成品备库策略（最大值）
        /// </summary>
        public decimal ProductBackUpMax { get; set; }

       /// <summary>
        /// 成品备库策略（最小值）
        /// </summary>
        public decimal ProductBackUpMin { get; set; }

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
    /// 合作伙伴精益与生产能力分页Dto
    /// </summary>
    public class NioPushProductioncapacityPagedQueryDto : PagerInfo
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
        /// 日期（格式为yyyy-MM-dd HH:mm:ss）
        /// </summary>
        public string? Date { get; set; }

        /// <summary>
        /// 推送状态;0：无需推送；1：待推送；2：已推送；3：推送失败；
        /// </summary>
        public PushStatusEnum? Status { get; set; }
    }

}
