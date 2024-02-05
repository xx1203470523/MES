using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 标准模板打印配置信息仓储接口
    /// </summary>
    public interface IProcLabelTemplateRelationRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLabelTemplateRelationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcLabelTemplateRelationEntity procLabelTemplateRelationEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procLabelTemplateRelationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcLabelTemplateRelationEntity> procLabelTemplateRelationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLabelTemplateRelationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcLabelTemplateRelationEntity procLabelTemplateRelationEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procLabelTemplateRelationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcLabelTemplateRelationEntity> procLabelTemplateRelationEntitys);

        /// <summary>
        /// 删除  
        /// 最好使用批量删除，可以设置更新人和更新时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcLabelTemplateRelationEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLabelTemplateRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procLabelTemplateRelationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLabelTemplateRelationEntity>> GetProcLabelTemplateRelationEntitiesAsync(ProcLabelTemplateRelationQuery procLabelTemplateRelationQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procLabelTemplateRelationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcLabelTemplateRelationEntity>> GetPagedInfoAsync(ProcLabelTemplateRelationPagedQuery procLabelTemplateRelationPagedQuery);

        /// <summary>
        /// 根据labelTemplateId获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcLabelTemplateRelationEntity> GetByLabelTemplateIdAsync(long labelTemplateId);

        /// <summary>
        /// 根据labelTemplateId硬删除对应数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteByLabelTemplateIdAsync(long labelTemplateId);
        #endregion
    }
}
