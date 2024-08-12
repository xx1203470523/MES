using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（Marking执行表）   
    /// manu_sfc_marking_execute
    /// @author xiaofei
    /// @date 2024-07-24 08:40:39
    /// </summary>
    public class ManuSfcMarkingExecuteEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// Marking信息表Id
        /// </summary>
        public long SfcMarkingId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 发现不良工序
        /// </summary>
        public long FoundBadProcedureId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public long UnqualifiedCodeId { get; set; }

        /// <summary>
        /// 应拦截工序
        /// </summary>
        public long ShouldInterceptProcedureId { get; set; }

        /// <summary>
        /// Marking类型(1-拦截 2-只标记)
        /// </summary>
        public MarkingTypeEnum MarkingType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        
    }
}
