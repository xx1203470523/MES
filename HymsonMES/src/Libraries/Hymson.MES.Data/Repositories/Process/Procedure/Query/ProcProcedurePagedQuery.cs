using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序表 分页参数
    /// </summary>
    public class ProcProcedurePagedQuery : PagerInfo
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 状态列表
        /// </summary>
        public SysDataStatusEnum[]? StatusArr { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ProcedureTypeEnum? Type { get; set; }

        /// <summary>
        /// 类型列表
        /// </summary>
        public ProcedureTypeEnum[]? TypeArr { get; set; }

        /// <summary>
        /// 描述 :资源类型名称 
        /// </summary>
        public string? ResTypeName { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;

        /// <summary>
        /// 工艺路线id
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? OrCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? OrName { get; set; }
    }
}
