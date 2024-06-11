using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Report
{
    /// <summary>
    /// 数据实体（降级品明细报表）   
    /// manu_downgrading_record
    /// @author huangjiayun
    /// @date 2023-09-11 02:27:40
    /// </summary>
    public class ManuDowngradingDetailReportView : BaseEntity
    {
        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 品级
        /// </summary>
        public string? Grade { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 录入人员
        /// </summary>
        public string? EntryPersonnel { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime? EntryTime { get; set; }

        /// <summary>
        /// 工单编号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum OrderType { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

    }
}
