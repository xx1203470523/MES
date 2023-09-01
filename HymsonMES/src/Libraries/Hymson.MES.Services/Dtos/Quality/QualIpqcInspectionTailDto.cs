using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 尾检检验单新增/更新Dto
    /// </summary>
    public record QualIpqcInspectionTailSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 生成条件单位
        /// </summary>
        public GenerateConditionUnitEnum TriggerCondition { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }
    }

    /// <summary>
    /// 尾检检验单Dto
    /// </summary>
    public record QualIpqcInspectionTailDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

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
        /// 不合格处理方式;1、让步 2、不合格
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

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorkOrderCode { get; set; }

        /// <summary>
        /// 工作中心（产线）
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 生成条件
        /// </summary>
        public int GenerateCondition { get; set; }

        /// <summary>
        /// 生成条件单位;1、小时 2、班次 3、批次 4、罐 5、卷
        /// </summary>
        public GenerateConditionUnitEnum GenerateConditionUnit { get; set; }

        /// <summary>
        /// 管控时间
        /// </summary>
        public int? ControlTime { get; set; }

        /// <summary>
        /// 管控时间单位;1、时 2、分
        /// </summary>
        public ControlTimeUnitEnum? ControlTimeUnit { get; set; }
    }

    /// <summary>
    /// 尾检检验单分页Dto
    /// </summary>
    public class QualIpqcInspectionTailPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string? InspectionOrder { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }


        /// <summary>
        /// 产品编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public InspectionStatusEnum? Status { get; set; }
    }

}
