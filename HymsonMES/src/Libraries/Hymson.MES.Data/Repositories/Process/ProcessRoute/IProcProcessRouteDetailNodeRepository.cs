/*
 *creator: Karl
 *
 *describe: 工艺路线工序节点明细表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:17:40
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工艺路线工序节点明细表仓储接口
    /// </summary>
    public interface IProcProcessRouteDetailNodeRepository
    {
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcessRouteDetailNodeEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcessRouteDetailNodeEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcessRouteDetailNodeView>> GetListAsync(ProcProcessRouteDetailNodeQuery query);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProcessRouteDetailNodeEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(List<ProcProcessRouteDetailNodeEntity> procProcessRouteDetailNodeEntitys);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procProcessRouteDetailNodeEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(List<ProcProcessRouteDetailNodeEntity> procProcessRouteDetailNodeEntitys);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(long[] ids);

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteByProcessRouteIdAsync(long id);
    }
}
