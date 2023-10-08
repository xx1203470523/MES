/*
 *creator: Karl
 *
 *describe: 开机参数表    Dto | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-05 04:22:20
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 开机参数表Dto
    /// </summary>
    public record ProcBootupparamDto : BaseEntityDto
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
        /// 开机配方Id
        /// </summary>
        public long RecipeId { get; set; }

       /// <summary>
        /// 标准参数Id
        /// </summary>
        public long ParamId { get; set; }

       /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

       /// <summary>
        /// 中心值
        /// </summary>
        public decimal? CenterValue { get; set; }

       /// <summary>
        /// 上限
        /// </summary>
        public decimal? MaxValue { get; set; }

       /// <summary>
        /// 下限
        /// </summary>
        public decimal? MinValue { get; set; }

       /// <summary>
        /// 说明
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
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long? IsDeleted { get; set; }

       
    }


    /// <summary>
    /// 开机参数表新增Dto
    /// </summary>
    public record ProcBootupparamCreateDto : BaseEntityDto
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
        /// 开机配方Id
        /// </summary>
        public long RecipeId { get; set; }

       /// <summary>
        /// 标准参数Id
        /// </summary>
        public long ParamId { get; set; }

       /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

       /// <summary>
        /// 中心值
        /// </summary>
        public decimal? CenterValue { get; set; }

       /// <summary>
        /// 上限
        /// </summary>
        public decimal? MaxValue { get; set; }

       /// <summary>
        /// 下限
        /// </summary>
        public decimal? MinValue { get; set; }

       /// <summary>
        /// 说明
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
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long? IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 开机参数表更新Dto
    /// </summary>
    public record ProcBootupparamModifyDto : BaseEntityDto
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
        /// 开机配方Id
        /// </summary>
        public long RecipeId { get; set; }

       /// <summary>
        /// 标准参数Id
        /// </summary>
        public long ParamId { get; set; }

       /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

       /// <summary>
        /// 中心值
        /// </summary>
        public decimal? CenterValue { get; set; }

       /// <summary>
        /// 上限
        /// </summary>
        public decimal? MaxValue { get; set; }

       /// <summary>
        /// 下限
        /// </summary>
        public decimal? MinValue { get; set; }

       /// <summary>
        /// 说明
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
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long? IsDeleted { get; set; }

       

    }

    /// <summary>
    /// 开机参数表分页Dto
    /// </summary>
    public class ProcBootupparamPagedQueryDto : PagerInfo
    {
    }
}
