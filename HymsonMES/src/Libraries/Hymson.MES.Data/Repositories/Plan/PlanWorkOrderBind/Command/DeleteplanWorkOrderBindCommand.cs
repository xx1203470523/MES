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
    /// 工单激活（物理删除） 根据资源和工单Ids删除
    /// </summary>
    public class DeleteplanWorkOrderBindCommand
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工单IDs
        /// </summary>
        public IEnumerable<long> WorkOrderIds { get; set; }
    }
}
