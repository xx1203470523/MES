/*
 *creator: Karl
 *
 *describe: 设备点检计划 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-16 02:14:30
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.EquSpotcheckPlan
{
    /// <summary>
    /// 设备点检计划 分页参数
    /// </summary>
    public class EquSpotcheckPlanPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 点检计划编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检计划名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 点检计划状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 设备编码 
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称 
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 点检执行人;用户中心UserId集合
        /// </summary>
        public string ExecutorIds { get; set; }

        /// <summary>
        /// 点检负责人;用户中心UserId集合
        /// </summary>
        public string LeaderIds { get; set; }
    }
}
