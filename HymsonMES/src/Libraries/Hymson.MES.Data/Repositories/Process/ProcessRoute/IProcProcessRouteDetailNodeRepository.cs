using Hymson.MES.Core.Domain.Process;

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
        /// 查询节点明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcProcessRouteDetailNodeEntity> GetByProcessRouteIdAsync(ProcProcessRouteDetailNodeQuery query);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcessRouteDetailNodeEntity> GetFirstProcedureByProcessRouteIdAsync(long processRouteId);

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
        Task<int> InsertRangeAsync(IEnumerable<ProcProcessRouteDetailNodeEntity> procProcessRouteDetailNodeEntitys);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procProcessRouteDetailNodeEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ProcProcessRouteDetailNodeEntity> procProcessRouteDetailNodeEntitys);

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
