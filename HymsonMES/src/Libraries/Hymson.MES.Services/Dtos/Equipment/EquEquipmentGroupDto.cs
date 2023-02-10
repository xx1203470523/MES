using Hymson.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 新增输入对象（设备组）
    /// </summary>
    public record EquEquipmentGroupCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 编码（设备组）
        /// </summary>
        [Required(ErrorMessage = "编码不能为空")]
        public string EquipmentGroupCode { get; set; }

        /// <summary>
        /// 名称（设备组）
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        public string EquipmentGroupName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 集合（设备注册）
        /// </summary>
        public IEnumerable<long> Equipments { get; set; }
    }

    /// <summary>
    /// 修改输入对象（设备组）
    /// </summary>
    public record EquEquipmentGroupModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名称（设备组）
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        public string EquipmentGroupName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 集合（设备注册）
        /// </summary>
        public IEnumerable<long> Equipments { get; set; }
    }

    /// <summary>
    /// 设备组Dto
    /// </summary>
    public record EquEquipmentGroupListDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码（设备组）
        /// </summary>
        public string EquipmentGroupCode { get; set; }

        /// <summary>
        /// 名称（设备组）
        /// </summary>
        public string EquipmentGroupName { get; set; }
    }

    /// <summary>
    /// 设备组分页Dto
    /// </summary>
    public class EquEquipmentGroupPagedQueryDto : PagerInfo
    {
        ///// <summary>
        ///// 描述 :站点编码 
        ///// 空值 : false  
        ///// </summary>
        //public string SiteCode { get; set; }
    }

    /// <summary>
    /// 自定义实体对象（设备组）
    /// </summary>
    public class EquEquipmentGroupDto
    {
        /// <summary>
        /// 信息（设备组）
        /// </summary>
        public EquEquipmentGroupListDto? Info { get; set; }

        /// <summary>
        /// 集合（设备）
        /// </summary>
        public IEnumerable<EquEquipmentBaseDto> Equipments { get; set; }
    }

    /// <summary>
    /// 查询对象（设备组）
    /// </summary>
    public record EquEquipmentGroupQueryDto : BaseEntityDto
    {
        /// <summary>
        /// 操作类型 1:add；2:edit；3:view；
        /// </summary>
        public string OperateType { get; set; } = "add";

        /// <summary>
        /// 设备组Id
        /// </summary>
        public long Id { get; set; }
    }
}
