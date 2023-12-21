using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 设备故障解决措施新增/更新Dto
    /// </summary>
    public record EquFaultSolutionSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障解决措施代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 故障解决措施名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 使用状态 0-禁用 1-启用
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 设备故障先
        /// </summary>
        public string Remark { get; set; }

    }

    /// <summary>
    /// 设备故障解决措施Dto
    /// </summary>
    public record EquFaultSolutionDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障解决措施代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 故障解决措施代码
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 使用状态 0-禁用 1-启用
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 设备故障先
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

    }

    /// <summary>
    /// 自定义实体列表（故障解决措施）
    /// </summary>
    public record EquFaultSolutionBaseDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码（设备注册）
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 名称（设备注册）
        /// </summary>
        public string Name { get; set; } = "";

    }

    /// <summary>
    /// 设备故障解决措施分页Dto
    /// </summary>
    public class EquFaultSolutionPagedQueryDto : PagerInfo
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

    }

}
