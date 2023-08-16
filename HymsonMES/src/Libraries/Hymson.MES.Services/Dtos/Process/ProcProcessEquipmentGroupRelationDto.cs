using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 设备组关联设备表新增/更新Dto
    /// </summary>
    public record ProcProcessEquipmentGroupRelationSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 备件ID
        /// </summary>
        public long EquipmentGroupId { get; set; }

       /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 设备组关联设备表Dto
    /// </summary>
    public record ProcProcessEquipmentGroupRelationDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 备件ID
        /// </summary>
        public long EquipmentGroupId { get; set; }

       /// <summary>
        /// 设备ID
        /// </summary>
        public string EquipmentId { get; set; }

    }

    /// <summary>
    /// 设备组关联设备表分页Dto
    /// </summary>
    public class ProcProcessEquipmentGroupRelationPagedQueryDto : PagerInfo { }

}
