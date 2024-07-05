using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.Query;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓储接口（工序配置子步骤表）
    /// </summary>
    public interface IProcProcedureSubstepRelationRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcProcedureSubstepRelationEntity entity);
        
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ProcProcedureSubstepRelationEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcProcedureSubstepRelationEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ProcProcedureSubstepRelationEntity> entities);

        /// <summary>
        /// 软删除  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);
        
        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 删除（物理删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteByProcedureIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcedureSubstepRelationEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureSubstepRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureSubstepRelationEntity>> GetEntitiesAsync(ProcProcedureSubstepRelationQuery query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcedureSubstepRelationEntity>> GetPagedListAsync(ProcProcedureSubstepRelationPagedQuery pagedQuery);

    }
}
