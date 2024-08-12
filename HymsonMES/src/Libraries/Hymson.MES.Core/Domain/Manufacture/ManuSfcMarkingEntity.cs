using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（Marking信息表）   
    /// manu_sfc_marking
    /// @author xiaofei
    /// @date 2024-07-24 08:40:23
    /// </summary>
    public class ManuSfcMarkingEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

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
        /// 状态(0-关闭 1-开启)
        /// </summary>
        public MarkingStatusEnum Status { get; set; }

        /// <summary>
        /// Marking类型(1-拦截 2-标记)
        /// </summary>
        public MarkingTypeEnum MarkingType { get; set; }

        /// <summary>
        /// 来源(1-直接录入 2-继承)
        /// </summary>
        public MarkingSourceTypeEnum SourceType { get; set; }

        /// <summary>
        /// 父级条码
        /// </summary>
        public string ParentSFC { get; set; }

        /// <summary>
        /// 原始条码
        /// </summary>
        public string OriginalSFC { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        
    }
}
