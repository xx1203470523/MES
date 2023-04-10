/*
 *creator: Karl
 *
 *describe: 生产过站面板    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:44:26
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 在制品维修面板Dto
    /// </summary>
    public record ManuFacePlateRepairDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 面板Id
        /// </summary>
        public long FacePlateId { get; set; }

       /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }
        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 是否修改资源
        /// </summary>
        public bool IsResourceEdit { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public string ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 是否修改工序
        /// </summary>
        public bool IsProcedureEdit { get; set; }

       /// <summary>
        /// 是否显示产品列表
        /// </summary>
        public bool IsShowProductList { get; set; }

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
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }


    /// <summary>
    /// 在制品维修新增Dto
    /// </summary>
    public record ManuFacePlateRepairCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

       /// <summary>
        /// 是否修改资源
        /// </summary>
        public bool? IsResourceEdit { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public string? ProcedureId { get; set; }

       /// <summary>
        /// 是否修改工序
        /// </summary>
        public bool? IsProcedureEdit { get; set; }

       /// <summary>
        /// 是否显示产品列表
        /// </summary>
        public bool? IsShowProductList { get; set; }
    }

    /// <summary>
    /// 在制品维修更新Dto
    /// </summary>
    public record ManuFacePlateRepairModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 面板Id
        /// </summary>
        public long FacePlateId { get; set; }

       /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

       /// <summary>
        /// 是否修改资源
        /// </summary>
        public bool? IsResourceEdit { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public string? ProcedureId { get; set; }

       /// <summary>
        /// 是否修改工序
        /// </summary>
        public bool? IsProcedureEdit { get; set; }

       /// <summary>
        /// 是否显示产品列表
        /// </summary>
        public bool? IsShowProductList { get; set; }
    }

    /// <summary>
    /// 在制品维修面板分页Dto
    /// </summary>
    public class ManuFacePlateRepairPagedQueryDto : PagerInfo
    {
    }
}
