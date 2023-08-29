using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 首检检验单新增Dto
    /// </summary>
    public record QualIpqcInspectionHeadSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 触发条件;1、开班检2、停机检3、换型检4、维修检5、换罐检
        /// </summary>
        public TriggerConditionEnum TriggerCondition { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }
    }

    /// <summary>
    /// 首检检验单Dto
    /// </summary>
    public record QualIpqcInspectionHeadDto : BaseEntityDto
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


    }

    /// <summary>
    /// 首检检验单分页查询参数Dto
    /// </summary>
    public class QualIpqcInspectionHeadPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string? InspectionOrder { get; set; }

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

    /// <summary>
    /// 状态变更Dto
    /// </summary>
    public record StatusChangeDto
    {
        /// <summary>
        /// 检验单Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public InspectionStatusEnum Status { get; set; }
    }

    /// <summary>
    /// 不合格处理Dto
    /// </summary>
    public record UnqualifiedHandleDto
    {
        /// <summary>
        /// 检验单Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 不合格处理方式;1、让步 2、？
        /// </summary>
        public HandMethodEnum? HandMethod { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 附件上传Dto
    /// </summary>
    public record AttachmentAddDto
    {
        /// <summary>
        /// 检验单Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 附件列表
        /// </summary>
        public IEnumerable<QualIpqcInspectionHeadAnnexSaveDto> Attachments { get; set; }
    }

    /// <summary>
    /// 检验单应检参数查询Dto
    /// </summary>
    public record SampleShouldInspectItemsQueryDto
    {
        /// <summary>
        /// 检验单Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string SampleCode { get; set; }
    }

    /// <summary>
    /// 检验单应检参数Dto
    /// </summary>
    public record SampleShouldInspectItemsDto : BaseEntityDto
    {
        /// <summary>
        /// IPQC检验项目参数Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// IPQC检验项目Id qual_ipqc_inspection 的id
        /// </summary>
        public long IpqcInspectionId { get; set; }

        /// <summary>
        /// 全检参数明细Id
        /// </summary>
        public long InspectionParameterGroupDetailId { get; set; }

        /// <summary>
        /// proc_parameter 参数Id
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public DataTypeEnum DataType { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        public decimal? UpperLimit { get; set; }

        /// <summary>
        /// 下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 中心值
        /// </summary>
        public decimal? CenterValue { get; set; }

        /// <summary>
        /// 录入次数
        /// </summary>
        public int? EnterNumber { get; set; }

        /// <summary>
        /// 是否设备采集;1、是 2、否
        /// </summary>
        public YesOrNoEnum? IsDeviceCollect { get; set; } = YesOrNoEnum.No;

        /// <summary>
        /// 顺序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 检验值(设备采集参数需要)
        /// </summary>
        public string InspectionValue { get; set; }
    }
}
