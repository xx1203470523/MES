using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.QualIpqcInspectionHead.View
{
    public class QualIpqcInspectionHeadView : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string InspectionOrder { get; set; }

        /// <summary>
        /// IPQC检验项目Id
        /// </summary>
        public long IpqcInspectionId { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 工单编号
        /// </summary>
        public long WorkOrderCode { get; set; }

        /// <summary>
        /// 工作中心（产线）
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 触发条件;1、开班检2、停机检3、换型检4、维修检5、换罐检
        /// </summary>
        public TriggerConditionEnum TriggerCondition { get; set; }

        /// <summary>
        /// 是否停机
        /// </summary>
        public TrueOrFalseEnum IsStop { get; set; }

        /// <summary>
        /// 管控时间
        /// </summary>
        public int? ControlTime { get; set; }

        /// <summary>
        /// 管控时间单位;1、时 2、分
        /// </summary>
        public ControlTimeUnitEnum? ControlTimeUnit { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public InspectionStatusEnum Status { get; set; }

        /// <summary>
        /// 报检人
        /// </summary>
        public string InspectionBy { get; set; }

        /// <summary>
        /// 报检时间
        /// </summary>
        public DateTime InspectionOn { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartOn { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompleteOn { get; set; }

        /// <summary>
        /// 关闭时间
        /// </summary>
        public DateTime? CloseOn { get; set; }

        /// <summary>
        /// 不合格处理方式;1、让步 2、？
        /// </summary>
        public HandMethodEnum? HandMethod { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string ProcessedBy { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? ProcessedOn { get; set; }
    }
}
