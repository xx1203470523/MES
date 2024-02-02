using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 跨工序时间管控分页查询Dto
    /// </summary>
    public record ProcProcedureTimeControlDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 管控标识
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 管控名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 起始工序
        /// </summary>
        public string FromProcedure { get; set; }

        /// <summary>
        /// 到达工序
        /// </summary>
        public string ToProcedure { get; set; }

        /// <summary>
        /// 上限时间（分）
        /// </summary>
        public int UpperLimitMinute { get; set; }

        /// <summary>
        /// 下限时间（分）
        /// </summary>
        public int LowerLimitMinute { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

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
    /// 跨工序时间管控新增Dto
    /// </summary>
    public record ProcProcedureTimeControlCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 管控标识
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 管控名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 起始工序Id
        /// </summary>
        public long FromProcedureId { get; set; }

        /// <summary>
        /// 到达工序Id
        /// </summary>
        public long ToProcedureId { get; set; }

        /// <summary>
        /// 上限时间（分）
        /// </summary>
        public int UpperLimitMinute { get; set; }

        /// <summary>
        /// 下限时间（分）
        /// </summary>
        public int LowerLimitMinute { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 跨工序时间管控更新Dto
    /// </summary>
    public record ProcProcedureTimeControlModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 管控标识
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 管控名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 起始工序Id
        /// </summary>
        public long FromProcedureId { get; set; }

        /// <summary>
        /// 到达工序Id
        /// </summary>
        public long ToProcedureId { get; set; }

        /// <summary>
        /// 上限时间（分）
        /// </summary>
        public int UpperLimitMinute { get; set; }

        /// <summary>
        /// 下限时间（分）
        /// </summary>
        public int LowerLimitMinute { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 跨工序时间管控分页查询Dto
    /// </summary>
    public record ProcProcedureTimeControlDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 管控标识
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 管控名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 起始工序Id
        /// </summary>
        public long FromProcedureId { get; set; }

        /// <summary>
        /// 到达工序Id
        /// </summary>
        public long ToProcedureId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 起始工序编码
        /// </summary>
        public string FromProcedure { get; set; }

        /// <summary>
        /// 到达工序编码
        /// </summary>
        public string ToProcedure { get; set; }

        /// <summary>
        /// 上限时间（分）
        /// </summary>
        public int UpperLimitMinute { get; set; }

        /// <summary>
        /// 下限时间（分）
        /// </summary>
        public int LowerLimitMinute { get; set; }

    }

    /// <summary>
    /// 跨工序时间管控分页Dto
    /// </summary>
    public class ProcProcedureTimeControlPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 管控标识
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 管控名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string? ProductCode { get; set; }

        /// <summary>
        /// 起始工序
        /// </summary>
        public string? FromProcedure { get; set; }

        /// <summary>
        /// 到达工序
        /// </summary>
        public string? ToProcedure { get; set; }

    }
}
