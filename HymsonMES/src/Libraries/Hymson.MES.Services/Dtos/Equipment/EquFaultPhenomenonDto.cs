using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 保存Dto（设备故障现象）
    /// </summary>
    public record EquFaultPhenomenonSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 故障现象代码
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 故障现象名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 设备故障先
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 关联原因
        /// </summary>
        public IEnumerable<long>? ReasonIds { get; set; }

    }

    /// <summary>
    /// Dto（设备故障现象）
    /// </summary>
    public record EquFaultPhenomenonDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障现象代码
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 故障现象名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 使用状态 0-禁用 1-启用
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 设备故障先
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; } = "";

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

    }

    /// <summary>
    /// 分页Dto（设备故障现象）
    /// </summary>
    public class EquFaultPhenomenonPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 故障现象代码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 故障现象名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 使用状态 0-禁用 1-启用
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

    }

    /// <summary>
    /// 设备故障现象查询Dto
    /// </summary>
    public class EquFaultPhenomenonQueryDto
    {
        /// <summary>
        /// 设备故障现象Id
        /// </summary>
        public long? Id { get; set; }
    }
}
