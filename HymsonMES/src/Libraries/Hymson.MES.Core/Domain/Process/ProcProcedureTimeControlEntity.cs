using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 跨工序时间管控，数据实体对象   
    /// proc_procedure_time_control
    /// </summary>
    public class ProcProcedureTimeControlEntity : BaseEntity
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
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
