using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 配方操作组维护新增/更新Dto
    /// </summary>
    public record ProcFormulaOperationGroupRelatiionSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 配方操作Id
        /// </summary>
        public long FormulaOperationId { get; set; }

       /// <summary>
        /// 配方操作组Id
        /// </summary>
        public long FormulaOperationGroupId { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public DateTime CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 配方操作组维护Dto
    /// </summary>
    public record ProcFormulaOperationGroupRelatiionDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 配方操作Id
        /// </summary>
        public long FormulaOperationId { get; set; }

       /// <summary>
        /// 配方操作组Id
        /// </summary>
        public long FormulaOperationGroupId { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public DateTime CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 配方操作组维护分页Dto
    /// </summary>
    public class ProcFormulaOperationGroupRelatiionPagedQueryDto : PagerInfo { }

}
