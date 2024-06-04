using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;

namespace Hymson.MES.Data.Repositories.Marking
{
    /// <summary>
    /// 数据实体（Marking拦截汇总表）   
    /// manu_sfc_marking_info
    /// @author Kongaomeng
    /// @date 2023-09-21 03:58:49
    /// </summary>
    public class MarkingRecordQueryReportView : BaseEntity
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 发现工序Id
        /// </summary>
        public long FindProcedureId { get; set; }

        /// <summary>
        /// 拦截工序Id
        /// </summary>
        public long AppointInterceptProcedureId { get; set; }

        /// <summary>
        /// 实际拦截工序Id
        /// </summary>
        public long InterceptProcedureId { get; set; }

        /// <summary>
        /// 拦截设备Id
        /// </summary>
        public long InterceptEquipmentId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称 
        /// </summary>
        public string UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 不合格状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 不合格类型
        /// </summary>
        public QualUnqualifiedCodeTypeEnum Type { get; set; }


        /// <summary>
        /// 数量
        /// </summary>
        public string Qty { get; set; }

        /// <summary>
        /// 拦截时间
        /// </summary>
        public DateTime? InterceptOn { get; set; }

        /// <summary>
        /// Marking录入人员
        /// </summary>
        public string MarkingCreatedBy { get; set; }

        /// <summary>
        /// Marking录入时间
        /// </summary>
        public DateTime MarkingCreatedOn { get; set; }

        /// <summary>
        /// Marking关闭人员
        /// </summary>
        public string MarkingClosedBy { get; set; }

        /// <summary>
        /// Marking录入时间
        /// </summary>
        public DateTime MarkingClosedOn { get; set; }

        /// <summary>
        /// 状态;0-关闭 1-开启
        /// </summary>
        public int? MarkingStatus { get; set; }

    }
}
