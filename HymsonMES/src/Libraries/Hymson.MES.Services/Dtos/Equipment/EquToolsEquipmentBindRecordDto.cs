using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 工具绑定设备操作记录表安装Dto
    /// </summary>
    public record EquToolsEquipmentBindRecordCreateDto : BaseEntityDto
    {
        ///// <summary>
        ///// 工具id equ_tools的id
        ///// </summary>
        //public long ToolId { get; set; }

        /// <summary>
        /// 工具编码
        /// </summary>
        public string ToolCode { get; set; }

        ///// <summary>
        ///// 设备id equ_equipment的 id
        ///// </summary>
        //public long EquipmentId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string Position { get; set; }
    }

    /// <summary>
    /// 工具绑定设备操作记录表新增/更新Dto
    /// </summary>
    public record EquToolsEquipmentBindRecordSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 本次使用寿命
        /// </summary>
        public decimal CurrentUsedLife { get; set; }

        /// <summary>
        /// 卸载原因 1、正常2、异常
        /// </summary>
        public EquUninstallReasonEnum? UninstallReason { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }="";
    }

    /// <summary>
    /// 查询详情实体
    /// </summary>
    public record EquToolsEquipmentBindRecordDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备id equ_equipment的 id
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
        /// 工具id equ_tools的id
        /// </summary>
        public long ToolId { get; set; }

        /// <summary>
        /// 工具编码
        /// </summary>
        public string? ToolCode { get; set; }

        /// <summary>
        /// 工具名称
        /// </summary>
        public string? ToolName { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 卸载原因 1、正常2、异常
        /// </summary>
        public EquUninstallReasonEnum? UninstallReason { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 额定寿命
        /// </summary>
        public decimal RatedLife { get; set; }

        /// <summary>
        /// 剩余寿命
        /// </summary>
        public decimal RemainingUsedLife { get; set; }

        /// <summary>
        /// 本次使用寿命
        /// </summary>
        public decimal CurrentUsedLife { get; set; }
    }

    /// <summary>
    /// 分页查询列表实体
    /// </summary>
    public record EquToolsEquipmentBindRecordViewDto : BaseEntityDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工具编码
        /// </summary>
        public string ToolCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? ToolName { get; set; }

        /// <summary>
        /// 工具类型
        /// </summary>
        public string ToolType { get; set; }

        /// <summary>
        /// 工具类型名称
        /// </summary>
        public string ToolTypeName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 安装时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 安装人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 额定寿命
        /// </summary>
        public decimal RatedLife { get; set; }

        /// <summary>
        /// 当前使用寿命
        /// </summary>
        public decimal? CurrentUsedLife { get; set; }

        /// <summary>
        /// 卸载人
        /// </summary>
        public string? UninstallBy { get; set; }

        /// <summary>
        /// 卸载时间
        /// </summary>
        public DateTime? UninstallOn { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 操作类型1、绑定 2、卸载
        /// </summary>
        public BindOperationTypeEnum OperationType { get; set; }
    }

    /// <summary>
    /// 工具绑定设备操作记录表分页Dto
    /// </summary>
    public class EquToolsEquipmentBindRecordPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工具编码
        /// </summary>
        public string? ToolCode { get; set; }

        /// <summary>
        /// 工具名称
        /// </summary>
        public string? ToolName { get; set; }

        /// <summary>
        /// 工具类型
        /// </summary>
        public string? ToolType { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string? Position { get; set; }

        /// <summary>
        /// 操作类型1、安装 2、卸载
        /// </summary>
        public BindOperationTypeEnum? OperationType { get; set; }

        /// <summary>
        /// 安装时间  时间范围  数组
        /// </summary>
        public DateTime[]? InstallTimeRange { get; set; }

        /// <summary>
        /// 卸载时间  时间范围  数组
        /// </summary>
        public DateTime[]? UninstallTimeRange { get; set; }
    }

}
