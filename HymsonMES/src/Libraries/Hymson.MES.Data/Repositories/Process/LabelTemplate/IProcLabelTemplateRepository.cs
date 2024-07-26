using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓库标签模板仓储接口
    /// </summary>
    public interface IProcLabelTemplateRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLabelTemplateEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcLabelTemplateEntity procLabelTemplateEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procLabelTemplateEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<ProcLabelTemplateEntity> procLabelTemplateEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLabelTemplateEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcLabelTemplateEntity procLabelTemplateEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procLabelTemplateEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<ProcLabelTemplateEntity> procLabelTemplateEntitys);

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
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcLabelTemplateEntity> GetByIdAsync(long id);
        Task<ProcLabelTemplateEntity> GetByNameAsync(ProcLabelTemplateByNameQuery query);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLabelTemplateEntity>> GetByIdsAsync(IEnumerable<long>  ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procLabelTemplateQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLabelTemplateEntity>> GetProcLabelTemplateEntitiesAsync(ProcLabelTemplateQuery procLabelTemplateQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procLabelTemplatePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcLabelTemplateEntity>> GetPagedInfoAsync(ProcLabelTemplatePagedQuery procLabelTemplatePagedQuery);

        /// <summary>
        /// 查询模板类型
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcLabelTemplateEntity> GetByTemplateTypeAsync(ProcLabelTemplateByTemplateTypeQuery query);
    }
}
