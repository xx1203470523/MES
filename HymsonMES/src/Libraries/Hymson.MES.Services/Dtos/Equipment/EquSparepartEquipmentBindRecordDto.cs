using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 工具绑定设备操作记录表新增/更新Dto
    /// </summary>
    public record EquSparepartEquipmentBindRecordCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 备件id equ_sparepart的id
        /// </summary>
        public long SparepartId { get; set; }

        /// <summary>
        /// 设备id equ_equipment的 id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string Position { get; set; }
    }

    /// <summary>
    /// 工具绑定设备操作记录表新增/更新Dto
    /// </summary>
    public record EquSparepartEquipmentBindRecordSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 卸载原因 1、正常2、异常
        /// </summary>
        public EquUninstallReasonEnum? UninstallReason { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
    }

    /// <summary>
    /// 工具绑定设备操作记录表Dto
    /// </summary>
    public record EquSparepartEquipmentBindRecordDto : BaseEntityDto
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
        /// 备件id equ_sparepart的id
        /// </summary>
        public long SparepartId { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparepartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? SparepartName { get; set; }

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
    }

    public record EquSparepartEquipmentBindRecordViewDto : BaseEntityDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparepartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? SparepartName { get; set; }

        /// <summary>
        /// 备件记录表id equ_sparepart_record 的Id
        /// </summary>
        public long SparepartRecordId { get; set; }

        /// <summary>
        /// 备件类型
        /// </summary>
        public string SparePartType { get; set; }

        /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName { get; set; }

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
    public class EquSparepartEquipmentBindRecordPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 备件编码
        /// </summary>
        public string? SparepartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? SparepartName { get; set; }

        /// <summary>
        /// 备件类型
        /// </summary>
        public string? SparePartType { get; set; }

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
