using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 配方操作新增/更新Dto
    /// </summary>
    public record ProcFormulaOperationSaveDto : BaseEntityDto
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
        /// 类型 0、新建 1、启用 2、保留 3、废除
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 操作类型 1、物料 2、物料组 3、固定值 5、设定值 6、参数值
        /// </summary>
        public FormulaOperationTypeEnum Type { get; set; }

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
    /// 配方操作表新增Dto
    /// </summary>
    public record AddFormulaOperationDto : BaseEntityDto
    {
        /// <summary>
        /// 配方操作
        /// </summary>
        public ProcFormulaOperationSaveDto FormulaOperation { get; set; }

        /// <summary>
        /// 设置值信息
        /// </summary>
        public List<ProcFormulaOperationSetSaveDto> FormulaOperationSetDtos { get; set; }

    }

    /// <summary>
    /// 配方操作Dto
    /// </summary>
    public record ProcFormulaOperationDto : BaseEntityDto
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
        /// 类型 0、新建 1、启用 2、保留 3、废除
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 操作类型 1、物料 2、物料组 3、固定值 5、设定值 6、参数值
        /// </summary>
        public FormulaOperationTypeEnum Type { get; set; }

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
    /// 配方操作分页Dto
    /// </summary>
    public class ProcFormulaOperationPagedQueryDto : PagerInfo 
    {
        /// <summary>
        /// 操作编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 操作状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public FormulaOperationTypeEnum? Type { get; set; }
    }

    /// <summary>
    /// 配方操作组获取配方操作分页Dto
    /// </summary>
    public class OperationGroupGetOperationPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 操作组Id
        /// </summary>
        public long? Id { get; set; }

    }

}
