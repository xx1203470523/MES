using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 配方操作设置新增/更新Dto
    /// </summary>
    public record ProcFormulaOperationSetSaveDto : BaseEntityDto
    {
       /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 设定值
        /// </summary>
        public int Value { get; set; }
  
    }

    /// <summary>
    /// 配方操作设置Dto
    /// </summary>
    public record ProcFormulaOperationSetDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 序号
        /// </summary>
        public int Serial { get; set; }

       /// <summary>
        /// 配方操作Id
        /// </summary>
        public long FormulaOperationId { get; set; }

       /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 设定值
        /// </summary>
        public string Value { get; set; }

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
    /// 配方操作设置分页Dto
    /// </summary>
    public class ProcFormulaOperationSetPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 配方操作Id
        /// </summary>
        public long? FormulaOperationId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
    }

}
