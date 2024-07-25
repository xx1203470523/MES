using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.ProcessRoute.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工艺路线表仓储接口
    /// </summary>
    public interface IProcProcessRouteRepository
    {
        /// <summary>
        /// 删除时查询启用和保留状态的不能删除
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcProcessRouteEntity> IsIsExistsEnabledAsync(ProcProcessRouteQuery query);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcessRouteEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcessRouteEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procProcessRouteQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcessRouteEntity>> GetProcProcessRouteEntitiesAsync(ProcProcessRouteQuery procProcessRouteQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procProcessRoutePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcessRouteEntity>> GetPagedInfoAsync(ProcProcessRoutePagedQuery procProcessRoutePagedQuery);

        /// <summary>
        /// 判断工艺路线是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(ProcProcessRouteQuery query);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> ResetCurrentVersionAsync(ResetCurrentVersionCommand command);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcessRouteEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcProcessRouteEntity procProcessRouteEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProcessRouteEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(List<ProcProcessRouteEntity> procProcessRouteEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProcessRouteEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcProcessRouteEntity procProcessRouteEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procProcessRouteEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(List<ProcProcessRouteEntity> procProcessRouteEntitys);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(DeleteCommand command);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);

        /// <summary>
        /// 根据编码获取工艺路线信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcProcessRouteEntity> GetByCodeAsync(ProcProcessRoutesByCodeQuery query);

        /// <summary>
        /// 根据编码获取工艺路线信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcessRouteEntity>> GetByCodesAsync(ProcProcessRoutesByCodeQuery param);

    }
}
