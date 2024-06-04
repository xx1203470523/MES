using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 
    /// </summary>
    public record WorkOrderStepControlViewDto : BaseEntityDto
    {
        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 步骤标识
        /// </summary>
        public string Serialno { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public string ProcessRout { get; set; }

        /// <summary>
        /// 排队中的数量
        /// </summary>
        public decimal PassDownQuantity { get; set; }

        /// <summary>
        /// 在制数量
        /// </summary>
        public decimal ProcessDownQuantity { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal FinishProductQuantity { get; set; }
    }

    /// <summary>
    /// 工单报告 分页参数
    /// </summary>
    public class WorkOrderStepControlPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 步骤标识
        /// </summary>
        public string Serialno { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public string ProcessRout { get; set; }

        /// <summary>
        /// 排队中的数量
        /// </summary>
        public decimal PassDownQuantity { get; set; }

        /// <summary>
        /// 在制数量
        /// </summary>
        public decimal ProcessDownQuantity { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal FinishProductQuantity { get; set; }
    }

    /// <summary>
    /// 工单报告 分页参数
    /// </summary>
    public class WorkOrderStepControlOptimizePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工单编码
        /// </summary>
        public string? OrderCode { get; set; }
    }
}
