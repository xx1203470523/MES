using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备故障原因表仓储接口
    /// </summary>
    public interface IEquFaultReasonRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>entities
        /// <returns></returns>
        Task<int> InsertAsync(EquFaultReasonEntity entity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<EquFaultReasonEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquFaultReasonEntity entity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<EquFaultReasonEntity> entities);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);

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
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquFaultReasonEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquFaultReasonEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquFaultReasonEntity> GetByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquFaultReasonEntity>> GetEntitiesAsync(EntityByStatusQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquFaultReasonEntity>> GetPagedListAsync(EquFaultReasonPagedQuery pagedQuery);



        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteByParentIdAsync(DeleteByParentIdCommand command);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRelationsAsync(IEnumerable<EquFaultReasonSolutionRelationEntity> entities);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquFaultReasonSolutionRelationEntity>> GetSolutionRelationEntitiesAsync(EntityByParentIdQuery query);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquFaultPhenomenonReasonRelationEntity>> GetPhenomenonRelationEntitiesAsync(EntityByParentIdQuery query);

    }
}
