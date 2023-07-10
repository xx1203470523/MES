using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Report
{
    public class ProductTraceReportDto
    {

    }

    #region 产品追溯查询
    /// <summary>
    /// 追溯报表查询
    /// </summary>
    public class ProductTracePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// true 正向，false 反向
        /// 默认正向追溯
        /// </summary>
        public bool TraceDirection { get; set; } = true;
    }
    /// <summary>
    /// 流转表追溯信息
    /// </summary>
    public record ManuSfcCirculationViewDto : BaseEntityDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 当前工序id
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
        public long? ResourceId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipentName { get; set; }

        /// <summary>
        /// 扣料上料点id
        /// </summary>
        public long? FeedingPointId { get; set; }

        /// <summary>
        /// 流转前条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 流转前工单id
        /// </summary>
        public long WorkOrderId { get; set; }
        /// <summary>
        /// 工单编码
        /// </summary>
        public string WorkOrderCode { get; set; }

        /// <summary>
        /// 流转前产品id
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 流转后条码信息
        /// </summary>
        public string CirculationBarCode { get; set; }

        /// <summary>
        /// 流转条码数量
        /// </summary>
        public decimal? CirculationQty { get; set; }

        /// <summary>
        /// 流转类型;1：拆分；2：合并；3：转换;4：消耗;5：拆解;6：组件添加 7.换件
        /// </summary>
        public SfcCirculationTypeEnum CirculationType { get; set; }

        /// <summary>
        /// 是否拆解
        /// </summary>
        public TrueOrFalseEnum IsDisassemble { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 合并时绑定位置
        /// </summary>
        public int? Location { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedOn { get; set; }
    }
    #endregion

    #region 产品参数查询
    public class ManuProductPrameterPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public ParameterTypeEnum? ParameterType { get; set; }
        /// <summary>
        ///采集开始时间
        ///CreatedOn
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        ///采集结束时间
        ///CreatedOn
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        ///设备上报开始时间
        ///CreatedOn
        /// </summary>
        public DateTime? LocalTimeStartTime { get; set; }
        /// <summary>
        ///设备上报结束时间
        ///CreatedOn
        /// </summary>
        public DateTime? LocalTimeEndTime { get; set; }
    }

    public record ManuProductParameterViewDto : BaseEntityDto
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 工序ID
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
        /// 资源ID
        /// </summary>
        public string ResourceId { get; set; }
        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }
        /// <summary>
        /// 工单ID
        /// </summary>
        public long WorkOrderId { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 设备本地时间
        /// </summary>
        public DateTime LocalTime { get; set; }
        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string ParameterValue { get; set; }
        /// <summary>
        /// 参数单位
        /// </summary>
        public string ParameterUnit { get; set; }
        /// <summary>
        /// 步骤ID，出站步骤ID
        /// </summary>
        public long StepId { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public ParameterTypeEnum ParameterType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    #endregion

    #region 条码履历（步骤）
    public class ManuSfcStepPagedQueryDto : PagerInfo
    {
        public string SFC { get; set; }
    }
    public record ManuSfcStepViewDto : BaseEntityDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 产品信息
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 当前数量
        /// </summary>
        public decimal Qty { get; set; }

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
        /// 资源ID
        /// </summary>
        public string ResourceId { get; set; }
        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 步骤类型; 跟枚举的对应不上了，具体以枚举的为准
        /// </summary>
        public ManuSfcStepTypeEnum Operatetype { get; set; }

        /// <summary>
        /// 当前状态;1：排队；2：激活；3：完工；
        /// </summary>
        public SfcProduceStatusEnum CurrentStatus { get; set; }

        /// <summary>
        /// 复投次数
        /// </summary>
        public int? RepeatedCount { get; set; }

        public bool? IsRepair { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public int? Passed { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
    #endregion

    #region 生产工艺
    public class ProcSfcProcessRoutePagedQueryDto : PagerInfo
    {
        public string SFC { get; set; }
    }

    public record ProcSfcProcessRouteViewDto : BaseEntityDto
    {
        public string SFC { get; set; }

        /// <summary>
        /// 当前数量
        /// </summary>
        public decimal? Qty { get; set; }

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
        /// 当前状态;1：排队；2：激活；3：完工；
        /// </summary>
        public SfcProduceStatusEnum? CurrentStatus { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public int? Passed { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// 进站时间
        /// </summary>
        public DateTime? InBountTime { get; set; }
        /// <summary>
        /// 出站时间
        /// </summary>
        public DateTime? OutBountTime { get; set; }
    }

    #endregion

    #region 工单信息
    public class ProductTracePlanWorkOrderPagedQueryDto : PagerInfo
    {
        public string SFC { get; set; }
    }
    public record ProductTracePlanWorkOrderViewDto : BaseEntityDto
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum Type { get; set; }
        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：锁定；6：暂停中；
        /// </summary>
        public PlanWorkOrderStatusEnum Status { get; set; }
        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal Qty { get; set; }
        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string WorkCenterName { get; set; }
        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }
        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanStartTime { get; set; }
        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? PlanEndTime { get; set; }
        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? RealStart { get; set; }
        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? RealEnd { get; set; }
    }
    #endregion
}
