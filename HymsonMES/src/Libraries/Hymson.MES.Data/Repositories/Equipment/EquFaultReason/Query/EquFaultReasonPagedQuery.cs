/*
 *creator: pengxin
 *
 *describe: 设备故障原因表 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-02-28 15:15:20
 */
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
        public string FaultReasonCode { get; set; }

        /// <summary>
        /// 名称（设备故障原因）
        /// </summary>
        public string FaultReasonName { get; set; }

        /// <summary>
        /// 故障原因状态（字典定义）
        /// </summary>
        public SysDataStatusEnum? UseStatus { get; set; }

        /// <summary>
        /// 描述（设备故障原因）
        /// </summary>
        public string Remark { get; set; }
    }
}
