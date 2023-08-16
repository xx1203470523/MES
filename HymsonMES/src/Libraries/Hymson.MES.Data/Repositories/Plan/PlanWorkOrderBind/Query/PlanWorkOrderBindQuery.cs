/*
 *creator: Karl
 *
 *describe: 工单激活（物理删除） 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-12 11:14:23
 */

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单激活（物理删除） 查询参数
    /// </summary>
    public class PlanWorkOrderBindQuery
    {
        public long SiteId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }


    }


    /// <summary>
    /// 工单激活（物理删除） 查询参数(根据资源ID)
    /// </summary>
    public class PlanWorkOrderBindByResourceIdQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }
    }
}
