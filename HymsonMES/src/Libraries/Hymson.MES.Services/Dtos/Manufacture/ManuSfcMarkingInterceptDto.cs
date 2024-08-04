using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// Marking拦截记录表新增/更新Dto
    /// </summary>
    public record ManuSfcMarkingInterceptSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// Marking信息表Id
        /// </summary>
        public long SfcMarkingId { get; set; }

       /// <summary>
        /// 拦截工序
        /// </summary>
        public long InterceptProcedureId { get; set; }

       /// <summary>
        /// 拦截设备
        /// </summary>
        public long InterceptEquipmentId { get; set; }

       /// <summary>
        /// 拦截资源
        /// </summary>
        public long InterceptResourceId { get; set; }

       /// <summary>
        /// 拦截时间
        /// </summary>
        public DateTime InterceptOn { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否删除
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// Marking拦截记录表Dto
    /// </summary>
    public record ManuSfcMarkingInterceptDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// Marking信息表Id
        /// </summary>
        public long SfcMarkingId { get; set; }

       /// <summary>
        /// 拦截工序
        /// </summary>
        public long InterceptProcedureId { get; set; }

       /// <summary>
        /// 拦截设备
        /// </summary>
        public long InterceptEquipmentId { get; set; }

       /// <summary>
        /// 拦截资源
        /// </summary>
        public long InterceptResourceId { get; set; }

       /// <summary>
        /// 拦截时间
        /// </summary>
        public DateTime InterceptOn { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否删除
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// Marking拦截记录表分页Dto
    /// </summary>
    public class ManuSfcMarkingInterceptPagedQueryDto : PagerInfo { }

}
