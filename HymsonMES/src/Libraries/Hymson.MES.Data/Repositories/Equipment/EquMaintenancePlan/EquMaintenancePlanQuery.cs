/*
 *creator: Karl
 *
 *describe: 设备点检计划 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-16 02:14:30
 */

namespace Hymson.MES.Data.Repositories.EquMaintenancePlan
{
    /// <summary>
    /// 设备点检计划 查询参数
    /// </summary>
    public class EquMaintenancePlanQuery
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
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }
}
