using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 配方操作组新增/更新Dto
    /// </summary>
    public record ProcFormulaOperationGroupSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 配方操作编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 配方操作名称
        /// </summary>
        public string Name { get; set; }

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
    /// 配方操作组Dto
    /// </summary>
    public record ProcFormulaOperationGroupDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 配方操作组编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 配方操作组名称
        /// </summary>
        public string Name { get; set; }

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
    /// 配方操作组分页Dto
    /// </summary>
    public class ProcFormulaOperationGroupPagedQueryDto : PagerInfo 
    {
        /// <summary>
        /// 配方操作组编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 配方操作组名称
        /// </summary>
        public string? Name { get; set; }
    }

    /// <summary>
    /// 配方操作组表新增Dto
    /// </summary>
    public record AddFormulaOperationGroupDto : BaseEntityDto
    {
        /// <summary>
        /// 配方操作
        /// </summary>
        public ProcFormulaOperationGroupSaveDto FormulaOperationGroup { get; set; }

        /// <summary>
        /// 设置值信息
        /// </summary>
        public List<ProcFormulaOperationSaveDto> FormulaOperationDtos { get; set; }

    }
}
