using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 作业表Dto
    /// @author admin
    /// @date 2023-02-21
    /// </summary>
    public record InteJobDto : BaseEntityDto
    {
        /// <summary>
        /// 所属站点代码 
        /// </summary>
        public string SiteId { get; set; }

        /// <summary>
        /// ID 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 作业编号 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 作业名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类程序 
        /// </summary>
        public string ClassProgram { get; set; }

        /// <summary>
        /// 备注 
        /// </summary>
        public string Remark { get; set; } = "";

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
        public byte IsDeleted { get; set; }
    }

    /// <summary>
    /// 作业表新增Dto
    /// @author admin
    /// @date 2023-02-21
    /// </summary>
    public record InteJobCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 作业编号 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 作业名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类程序 
        /// </summary>
        public string ClassProgram { get; set; }

        /// <summary>
        /// 备注 
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 作业规则
        /// </summary>
        public List<InteJobConfigDto> ConfigList { get; set; }
    }

    /// <summary>
    /// 作业表更新Dto
    /// @author admin
    /// @date 2023-02-21
    /// </summary>
    public record InteJobModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 作业名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类程序 
        /// </summary>
        public string ClassProgram { get; set; }

        /// <summary>
        /// 备注 
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 作业规则
        /// </summary>
        public List<InteJobConfigDto> ConfigList { get; set; }
    }

    /// <summary>
    /// 作业表分页Dto
    /// @author admin
    /// @date 2023-02-21
    /// </summary>
    public class InteJobPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 作业编号 
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 作业名称 
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 类程序 
        /// </summary>
        public string? ClassProgram { get; set; }
    }

    /// <summary>
    /// 作业配置Dto
    /// </summary>
    public record InteJobConfigDto : BaseEntityDto
    {
        /// <summary>
        /// 设定值
        /// </summary>
        public string SetValue { get; set; } = "";

        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName { get; set; }
    }
}

