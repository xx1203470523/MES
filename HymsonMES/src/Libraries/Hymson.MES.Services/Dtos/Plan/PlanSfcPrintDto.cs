using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Plan
{
    /// <summary>
    /// 条码表Dto
    /// </summary>
    public class PlanSfcPrintPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public PlanSFCUsedStatusEnum? IsUsed { get; set; }
    }

    /// <summary>
    /// 条码打印Dto
    /// </summary>
    public record PlanSfcPrintDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }
        /// <summary>
        /// 使用状态
        /// </summary>
        public PlanSFCUsedStatusEnum IsUsed { get; set; }
        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

    }

    /// <summary>
    /// 条码打印新增Dto
    /// </summary>
    public record PlanSfcPrintCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 关联工单
        /// </summary>
        public long RelevanceWorkOrderId { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanSFCReceiveTypeEnum ReceiveType { get; set; }


    }

}
