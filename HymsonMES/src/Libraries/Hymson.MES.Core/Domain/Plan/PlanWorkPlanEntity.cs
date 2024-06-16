using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Plan
{
    /// <summary>
    /// 生产计划数据实体对象
    /// @author Czhipu
    /// @date 2024-06-16
    /// </summary>
    public partial class PlanWorkPlanEntity : BaseEntity
    {
        /// <summary>
        /// 计划单号
        /// </summary>
        public string PlanCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string ProductVersion { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// BOM编码
        /// </summary>
        public long BomCode { get; set; }

        /// <summary>
        /// BOM版本
        /// </summary>
        public string BomVersion { get; set; }

        /// <summary>
        /// bomId
        /// </summary>
        public long BomId { get; set; }

        /// <summary>
        /// 计划类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 计划状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 需求单号
        /// </summary>
        public string Requirementnumber { get; set; } = "";

        /// <summary>
        /// 超生产比例;默认是0，若允许超产，则写超产的%比例
        /// </summary>
        public decimal OverScale { get; set; } = 0;

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
