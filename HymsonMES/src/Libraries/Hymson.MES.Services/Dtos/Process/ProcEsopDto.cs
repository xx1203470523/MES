/*
 *creator: Karl
 *
 *describe: ESOP    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-11-02 02:39:53
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// ESOPDto
    /// </summary>
    public record ProcEsopDto : BaseEntityDto
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
        /// 物料Id  proc_material 表Id
        /// </summary>
        public long? MaterialId { get; set; }

       /// <summary>
        /// 工序Id  proc_procedure表 Id
        /// </summary>
        public long? ProcedureId { get; set; }

       /// <summary>
        /// 状态 0-未启用  1-启用
        /// </summary>
        public bool? Status { get; set; }

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
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }


    /// <summary>
    /// ESOP新增Dto
    /// </summary>
    public record ProcEsopCreateDto : BaseEntityDto
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
        /// 物料Id  proc_material 表Id
        /// </summary>
        public long? MaterialId { get; set; }

       /// <summary>
        /// 工序Id  proc_procedure表 Id
        /// </summary>
        public long? ProcedureId { get; set; }

       /// <summary>
        /// 状态 0-未启用  1-启用
        /// </summary>
        public bool? Status { get; set; }

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
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// ESOP更新Dto
    /// </summary>
    public record ProcEsopModifyDto : BaseEntityDto
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
        /// 物料Id  proc_material 表Id
        /// </summary>
        public long? MaterialId { get; set; }

       /// <summary>
        /// 工序Id  proc_procedure表 Id
        /// </summary>
        public long? ProcedureId { get; set; }

       /// <summary>
        /// 状态 0-未启用  1-启用
        /// </summary>
        public bool? Status { get; set; }

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
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       

    }

    /// <summary>
    /// ESOP分页Dto
    /// </summary>
    public class ProcEsopPagedQueryDto : PagerInfo
    {
    }
}
