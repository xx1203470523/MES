using Hymson.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 设备关联硬件新增实体
    /// </summary>
    public record EquEquipmentLinkHardwareCreateDto : BaseEntityDto
    {
        ///// <summary>
        ///// 设备id
        ///// </summary>
        //public long EquipmentId { get; set; }

        /// <summary>
        /// 硬件唯一标识
        /// </summary>
        public string HardwareCode { get; set; }

        /// <summary>
        /// 硬件类型
        /// </summary>
        public string HardwareType { get; set; }
    }

    /// <summary>
    /// 设备关联硬件更新实体
    /// @author Karl
    /// @date 2022-12-09
    /// </summary>
    public record EquEquipmentLinkHardwareModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 硬件唯一标识
        /// </summary>
        public string HardwareCode { get; set; }

        /// <summary>
        /// 硬件类型
        /// </summary>
        public string HardwareType { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [Required(ErrorMessage = "操作类型不可为空")]
        [Range(1, 3)]
        public int OperationType { get; set; }
    }

    /// <summary>
    /// 设备关联硬件查询实体类
    /// </summary>
    public record EquEquipmentLinkHardwareBaseDto : BaseEntityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 描述 :设备id（equ_equipment表id） 
        /// 空值 : false  
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 描述 :硬件设备唯一标识 
        /// 空值 : false  
        /// </summary>
        public string HardwareCode { get; set; }

        /// <summary>
        /// 描述 :硬件类型（字段名称：equ_hardware_type） 
        /// 空值 : false  
        /// </summary>
        public string HardwareType { get; set; }

        /// <summary>
        /// 描述 :设备故障先 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 设备关联硬件查询实体类
    /// </summary>
    public record EquEquipmentLinkHardwareDto : BaseEntityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 描述 :设备id（equ_equipment表id） 
        /// 空值 : false  
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 描述 :硬件设备唯一标识 
        /// 空值 : false  
        /// </summary>
        public string HardwareCode { get; set; }

        /// <summary>
        /// 描述 :硬件类型（字段名称：equ_hardware_type） 
        /// 空值 : false  
        /// </summary>
        public string HardwareType { get; set; }

        /// <summary>
        /// 描述 :设备故障先 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = "";

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }

    /// <summary>
    /// 设备关联硬件查询对象
    /// </summary>
    public class EquEquipmentLinkHardwarePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 所属设备ID
        /// </summary>
        public long EquipmentId { get; set; }
    }
}
