using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备故障原因表 分页故障原因
    /// </summary>
    public class EquFaultReasonPagedQuery : PagerInfo  
    {
        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码（设备故障原因）
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称（设备故障原因）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 故障原因状态（字典定义）
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 描述（设备故障原因）
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 故障原因Id列表
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }
    }
}
