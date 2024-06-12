using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
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
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 工具id
        /// </summary>
        public long? ToolId { get; set; }

       /// <summary>
        /// 工具记录表id equ_tools_record 的Id
        /// </summary>
        public long? ToolsRecordId { get; set; }

       /// <summary>
        /// 设备id equ_equipment的 id
        /// </summary>
        public long? EquipmentId { get; set; }

       /// <summary>
        /// 设备记录id equ_equipment_record
        /// </summary>
        public long EquipmentRecordId { get; set; }

       /// <summary>
        /// 位置号
        /// </summary>
        public string Position { get; set; }

       /// <summary>
        /// 操作类型1、绑定 2、卸载
        /// </summary>
        public bool? OperationType { get; set; }

       /// <summary>
        /// 卸载原因 1、正常2、异常
        /// </summary>
        public bool? UninstallReason { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 卸载人
        /// </summary>
        public string UninstallBy { get; set; }

       /// <summary>
        /// 卸载时间
        /// </summary>
        public DateTime? UninstallOn { get; set; }

       
    }

    /// <summary>
    /// 工具绑定设备操作记录表Dto
    /// </summary>
    public record EquToolsEquipmentBindRecordDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 工具id
        /// </summary>
        public long? ToolId { get; set; }

       /// <summary>
        /// 工具记录表id equ_tools_record 的Id
        /// </summary>
        public long? ToolsRecordId { get; set; }

       /// <summary>
        /// 设备id equ_equipment的 id
        /// </summary>
        public long? EquipmentId { get; set; }

       /// <summary>
        /// 设备记录id equ_equipment_record
        /// </summary>
        public long EquipmentRecordId { get; set; }

       /// <summary>
        /// 位置号
        /// </summary>
        public string Position { get; set; }

       /// <summary>
        /// 操作类型1、绑定 2、卸载
        /// </summary>
        public bool? OperationType { get; set; }

       /// <summary>
        /// 卸载原因 1、正常2、异常
        /// </summary>
        public bool? UninstallReason { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 卸载人
        /// </summary>
        public string UninstallBy { get; set; }

       /// <summary>
        /// 卸载时间
        /// </summary>
        public DateTime? UninstallOn { get; set; }

       
    }

    /// <summary>
    /// 工具绑定设备操作记录表分页Dto
    /// </summary>
    public class EquToolsEquipmentBindRecordPagedQueryDto : PagerInfo { }

}
