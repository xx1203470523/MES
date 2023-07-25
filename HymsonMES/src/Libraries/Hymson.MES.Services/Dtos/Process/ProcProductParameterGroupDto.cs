using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 产品工序参数组新增/更新Dto
    /// </summary>
    public record ProcProductParameterGroupSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 参数集编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 参数集名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

       /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

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
    /// 产品工序参数组Dto
    /// </summary>
    public record ProcProductParameterGroupDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 参数集编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 参数集名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

       /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

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
    /// 产品工序参数组分页Dto
    /// </summary>
    public class ProcProductParameterGroupPagedQueryDto : PagerInfo { }

}
