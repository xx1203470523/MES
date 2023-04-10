using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工艺路线工序节点关系明细表(前节点多条就存多条)仓储接口
    /// </summary>
    public interface IProcProcessRouteDetailLinkRepository
    {
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcessRouteDetailLinkEntity> GetByIdAsync(long id);

        /// <summary>
        /// 获某工序对应的下一工序
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcProcessRouteDetailLinkEntity> GetProcessRouteDetailLinkAsync(ProcProcessRouteDetailLinkQuery query);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcessRouteDetailLinkEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procProcessRouteDetailLinkQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcessRouteDetailLinkEntity>> GetListAsync(ProcProcessRouteDetailLinkQuery procProcessRouteDetailLinkQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procProcessRouteDetailLinkPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcessRouteDetailLinkEntity>> GetPagedInfoAsync(ProcProcessRouteDetailLinkPagedQuery procProcessRouteDetailLinkPagedQuery);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcessRouteDetailLinkEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcProcessRouteDetailLinkEntity procProcessRouteDetailLinkEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProcessRouteDetailLinkEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ProcProcessRouteDetailLinkEntity> procProcessRouteDetailLinkEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProcessRouteDetailLinkEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcProcessRouteDetailLinkEntity procProcessRouteDetailLinkEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procProcessRouteDetailLinkEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ProcProcessRouteDetailLinkEntity> procProcessRouteDetailLinkEntitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

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
