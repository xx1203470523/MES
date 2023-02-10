using Hymson.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 设备关联API新增实体
    /// </summary>
    public record EquEquipmentLinkApiCreateDto : BaseEntityDto
    {
        ///// <summary>
        ///// 设备id
        ///// </summary>
        //public long EquipmentId { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 接口名称（类型）
        /// </summary>
        public string ApiType { get; set; }
    }

    /// <summary>
    /// 设备关联API更新实体
    /// @author Karl
    /// @date 2022-12-09
    /// </summary>
    public record EquEquipmentLinkApiModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 接口地址
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 接口名称（类型）
        /// </summary>
        public string ApiType { get; set; }

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
    /// 设备关联API查询实体类
    /// </summary>
    public record EquEquipmentLinkApiBaseDto : BaseEntityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备id（equ_equipment表id） 
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 接口地址 
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 接口类型（字段名称：equ_api_type） 
        /// </summary>
        public string ApiType { get; set; }

        /// <summary>
        /// 设备故障先 
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 设备关联API查询实体类
    /// </summary>
    public record EquEquipmentLinkApiDto : BaseEntityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 设备id（equ_equipment表id） 
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 接口地址 
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 接口类型（字段名称：equ_api_type） 
        /// </summary>
        public string ApiType { get; set; }

        /// <summary>
        /// 设备故障先 
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
    /// 设备关联API查询对象
    /// </summary>
    public class EquEquipmentLinkApiPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 所属设备ID
        /// </summary>
        public long EquipmentId { get; set; }
    }
}
