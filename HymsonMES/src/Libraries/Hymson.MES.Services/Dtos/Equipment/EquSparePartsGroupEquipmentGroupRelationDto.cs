using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 备件类型和设备关联关系表新增/更新Dto
    /// </summary>
    public record EquSparePartsGroupEquipmentGroupRelationSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// equ_spare_parts_group 的id
        /// </summary>
        public long? SparePartsGroupId { get; set; }

        /// <summary>
        /// equ_spare_parts_group 的id
        /// </summary>
        public long? SparePartTypeId { get; set; }

        /// <summary>
        /// equ_equipment_grou  的id
        /// </summary>
        public long? EquipmentGroupId { get; set; }

        /// <summary>
        /// 设备组编码
        /// </summary>
        public string EquipmentGroupCode { get; set; }

        /// <summary>
        /// 设备组名称
        /// </summary>
        public string EquipmentGroupName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       
    }

    /// <summary>
    /// 备件类型和设备关联关系表Dto
    /// </summary>
    public record EquSparePartsGroupEquipmentGroupRelationDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// equ_spare_parts_group 的id
        /// </summary>
        public long? SparePartsGroupId { get; set; }

        /// <summary>
        /// equ_spare_parts_group 的id
        /// </summary>
        public long? SparePartTypeId { get; set; }

        /// <summary>
        /// equ_equipment_grou  的id
        /// </summary>
        public long? EquipmentGroupId { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       
    }

    /// <summary>
    /// 备件类型和设备关联关系表分页Dto
    /// </summary>
    public class EquSparePartsGroupEquipmentGroupRelationPagedQueryDto : PagerInfo { }

}
