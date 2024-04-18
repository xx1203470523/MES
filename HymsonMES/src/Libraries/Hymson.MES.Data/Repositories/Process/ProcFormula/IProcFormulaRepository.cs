using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Process.View;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓储接口（配方维护）
    /// </summary>
    public interface IProcFormulaRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcFormulaEntity entity);
        
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ProcFormulaEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcFormulaEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ProcFormulaEntity> entities);

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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcFormulaEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcFormulaEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcFormulaEntity>> GetEntitiesAsync(ProcFormulaQuery query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcFormulaView>> GetPagedListAsync(ProcFormulaPagedQuery pagedQuery);

        /// <summary>
        /// 更新某物料 的状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);

        /// <summary>
        /// 根据编码与版本获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcFormulaEntity> GetByCodeAndVersionAsync(ProcFormulaByCodeAndVersion query);

        #region 顷刻
        /// <summary>
        /// 获取配方列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<List<ProcFormulaListViewDto>> GetFormulaListAsync(ProcFormulaListQueryDto queryDto);

        /// <summary>
        /// 获取配方详情
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ProcFormulaDetailViewDto>> GetFormulaDetailAsync(ProcFormulaDetailQueryDto query);

        /// <summary>
        /// 获取激活版本
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcFormulaEntity> GetActivateByCodeAndVersionAsync(ProcFormulaByCodeAndVersion query);

        #endregion
    }
}