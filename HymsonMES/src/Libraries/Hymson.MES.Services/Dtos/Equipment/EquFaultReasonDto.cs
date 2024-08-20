using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 设备故障原因表新增Dto
    /// </summary>
    public record EquFaultReasonSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障原因代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 故障原因名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 关联解决措施
        /// </summary>
        public IEnumerable<long>? SolutionIds { get; set; }

    }

    /// <summary>
    /// 设备故障原因表Dto
    /// </summary>
    public record EquFaultReasonDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障原因代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 故障原因名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 故障原因状态（字典定义）
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public record CustomEquFaultReasonDto : EquFaultReasonDto
    {

    }

    /// <summary>
    /// 设备故障原因表分页Dto
    /// </summary>
    public class EquFaultReasonPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 编码（设备故障原因）
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称（设备故障原因）
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 故障原因状态（字典定义）
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 描述（设备故障原因）
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 故障现象编码
        /// </summary>
        public string? PhenomenonCode { get; set; } = "";
    }

    /// <summary>
    /// 设备故障原因表查询Dto
    /// </summary>
    public class EquFaultReasonQueryDto
    {
        /// <summary>
        /// Ids
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }
    }
}
