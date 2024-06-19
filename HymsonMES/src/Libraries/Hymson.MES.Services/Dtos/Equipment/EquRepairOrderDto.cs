/*
 *creator: Karl
 *
 *describe: 设备维修记录    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.EquRepairOrder
{
    /// <summary>
    /// 设备维修记录Dto
    /// </summary>
    public record EquRepairOrderDto : BaseEntityDto
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
        /// 维修
        /// </summary>
        public string RepairOrder { get; set; }

       /// <summary>
        /// 设备id equ_equipment的 id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 设备记录id equ_equipment_record
        /// </summary>
        public long EquipmentRecordId { get; set; }

       /// <summary>
        /// 故障时间
        /// </summary>
        public DateTime FaultTime { get; set; }

       /// <summary>
        /// 是否停机
        /// </summary>
        public TrueOrFalseEnum IsShutdown { get; set; }

       /// <summary>
        /// 状态 1、待维修 2、已维修3、已确认
        /// </summary>
        public EquRepairOrderStatusEnum Status { get; set; }

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

       
    }


    /// <summary>
    /// 设备维修记录新增Dto
    /// </summary>
    public record EquRepairOrderCreateDto : BaseEntityDto
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
        /// 维修
        /// </summary>
        public string RepairOrder { get; set; }

       /// <summary>
        /// 设备id equ_equipment的 id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 设备记录id equ_equipment_record
        /// </summary>
        public long EquipmentRecordId { get; set; }

       /// <summary>
        /// 故障时间
        /// </summary>
        public DateTime FaultTime { get; set; }

       /// <summary>
        /// 是否停机
        /// </summary>
        public TrueOrFalseEnum IsShutdown { get; set; }

       /// <summary>
        /// 状态 1、待维修 2、已维修3、已确认
        /// </summary>
        public EquRepairOrderStatusEnum Status { get; set; }

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

       
    }

    /// <summary>
    /// 设备维修记录更新Dto
    /// </summary>
    public record EquRepairOrderModifyDto : BaseEntityDto
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
        /// 维修
        /// </summary>
        public string RepairOrder { get; set; }

       /// <summary>
        /// 设备id equ_equipment的 id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 设备记录id equ_equipment_record
        /// </summary>
        public long EquipmentRecordId { get; set; }

       /// <summary>
        /// 故障时间
        /// </summary>
        public DateTime FaultTime { get; set; }

       /// <summary>
        /// 是否停机
        /// </summary>
        public TrueOrFalseEnum IsShutdown { get; set; }

       /// <summary>
        /// 状态 1、待维修 2、已维修3、已确认
        /// </summary>
        public EquRepairOrderStatusEnum Status { get; set; }

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

       

    }

    /// <summary>
    /// 设备维修记录分页Dto
    /// </summary>
    public class EquRepairOrderPagedQueryDto : PagerInfo
    {
    }
}
