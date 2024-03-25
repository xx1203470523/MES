using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{
    public record ProductionManagePanelReportDto : BaseEntityDto
    {
        /// <summary>
        /// 综合良率
        /// </summary>
        public decimal OverallYieldRate { get; set; }
        /// <summary>
        /// 计划达成率
        /// </summary>
        public decimal OverallPlanAchievingRate { get; set; }
        /// <summary>
        /// 日电芯消耗数量
        /// 计算首工序进站电芯
        /// </summary>
        public decimal DayConsume { get; set; }
        /// <summary>
        /// 线体名称
        /// </summary>
        public string? WorkLineName { get; set; }
        /// <summary>
        /// 完成率
        /// </summary>
        public decimal CompletedRate { get; set; }

        /// <summary>
        /// 1白班，0夜班
        /// </summary>
        public int DayShift { get; set; }
        /// <summary>
        /// 工艺路线编码
        /// </summary>
        public string? ProcessRouteCode { get; set; }
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string? ProcessRouteName { get; set; }
        /// <summary>
        /// 工单下达时间
        /// </summary>
        public string WorkOrderDownTime { get; set; }
        /// <summary>
        /// 工单号
        /// </summary>
        public string? WorkOrderCode { get; set; }
        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal? WorkOrderQty { get; set; }

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal CompletedQty { get; set; }

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal UnqualifiedQty { get; set; }

        /// <summary>
        /// 不良率
        /// </summary>
        public decimal UnqualifiedRate { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string? ProductCode { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string? ProductName { get; set; }
        /// <summary>
        /// 投入数量
        /// </summary>
        public decimal? InputQty { get; set; }
    }

    /// <summary>
    /// 工序完成数据查询
    /// </summary>
    public record ProcessCompletedDataQueryDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 工单号
        /// </summary>
        public long WorkOrderId { get; set; }
    }

    /// <summary>
    /// 工序完工数量
    /// </summary>
    public class ProcessCompletedDataDto
    {
        public long WorkOrderId { get; set; }

        public decimal CompletedQty { get; set; }
    }

    /// <summary>
    /// 模组信息统计区间Dto
    /// </summary>
    public class ProductionManagePanelModuleRangeDto
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string DateTimeRange { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public TimeSpan StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public TimeSpan EndTime { get; set; }
        /// <summary>
        /// true 白班 false夜班
        /// </summary>
        public bool IsDayShift { get; set; }
    }
    /// <summary>
    /// 模组信息Dto
    /// </summary>
    public record ProductionManagePanelModuleDto : BaseEntityDto
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 时间区间
        /// </summary>
        public string DateTimeRange { get; set; }
        /// <summary>
        /// 投入数
        /// </summary>
        public decimal InputQty { get; set; }
        /// <summary>
        /// 目标数
        /// </summary>
        public decimal TargetQty { get; set; }
        /// <summary>
        /// 达成率
        /// </summary>
        public decimal AchievingRate { get; set; }
    }

    /// <summary>
    /// 模组达成信息查询
    /// </summary>
    public record ModuleAchievingQueryDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 目标总数（未分段）
        /// </summary>
        public decimal TargetTotal { get; set; }
    }

    /// <summary>
    /// 设备稼动率查询
    /// </summary>
    public record EquipmentUtilizationRateQueryDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 设备理论加工周期
        /// 暂时只设置总理论值 默认100
        /// </summary>
        public decimal TheoryCycle { get; set; } = 100;
        /// <summary>
        /// 工作周期
        /// 默认20小时
        /// </summary>
        public decimal WorkingHours { get; set; } = 20;

        /// <summary>
        /// 设备编码
        /// </summary>
        public string[]? EquipmentCodes { get; set; }
    }

    /// <summary>
    /// 设备稼动率Dto
    /// </summary>
    public record EquipmentUtilizationRateDto : BaseEntityDto
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 稼动率
        /// </summary>
        public decimal UtilizationRate { get; set; }

        /// <summary>
        /// 设备运行状态
        /// </summary>
        public EquipmentStateEnum? EquipmentStatus { get; set; }

        /// <summary>
        /// 设备运行状态名称
        /// </summary>
        public string? EquipmentStatusName { get; set; }
    }

    /// <summary>
    /// 设备停机时长Dto
    /// </summary>
    public class EquipmentStopTimeDto
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 总停机秒数
        /// </summary>
        public decimal StopSeconds { get; set; }
    }

    /// <summary>
    /// 设备良品信息
    /// </summary>
    public class EquipmentYieldDto
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// 良品数
        /// </summary>
        public decimal YieldQty { get; set; }
    }

    /// <summary>
    /// 工序合格率Dto
    /// </summary>
    public record ProcessQualityRateDto : BaseEntityDto
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProccessCode { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// 直通率
        /// </summary>
        public decimal FirstPassYieldRate { get; set; }
        /// <summary>
        /// 良率
        /// </summary>
        public decimal YieldRate { get; set; }
    }

    /// <summary>
    /// 工序良品率
    /// </summary>
    public record ProcessYieldRateDto : BaseEntityDto
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProccessCode { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 良品率
        /// </summary>
        public decimal YieldRate { get; set; }

        /// <summary>
        /// 良品数量
        /// </summary>
        public decimal YieldQty { get; set; }

        /// <summary>
        /// 当月日期
        /// 如01,02
        /// </summary>
        public string Day { get; set; }
    }

    /// <summary>
    /// 工序良率查询
    /// </summary>
    public record ProcessQualityRateQueryDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string[] ProcedureCodes { get; set; }
    }

    /// <summary>
    /// 工序当月良品波动查询
    /// </summary>
    public record ProcessYieldRateQueryDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string[] ProcedureCodes { get; set; }
    }

    /// <summary>
    /// 工序指数查询
    /// </summary>
    public record ProcessIndicatorsQueryDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string[] ProcedureCodes { get; set; }
    }
    /// <summary>
    /// 工序指数
    /// </summary>
    public record ProcessIndicatorsDto : BaseEntityDto
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProccessCode { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 指数值(产量)
        /// </summary>
        public decimal Indicators { get; set; }
        /// <summary>
        /// 当月日期
        /// 如01,02
        /// </summary>
        public string Day { get; set; }
    }

}
