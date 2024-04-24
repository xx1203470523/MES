/*
 *creator: Karl
 *
 *describe: 降级规则仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-07 02:00:57
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 降级规则仓储接口
    /// </summary>
    public interface IManuDowngradingRuleRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuDowngradingRuleEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuDowngradingRuleEntity manuDowngradingRuleEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuDowngradingRuleEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuDowngradingRuleEntity> manuDowngradingRuleEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuDowngradingRuleEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuDowngradingRuleEntity manuDowngradingRuleEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuDowngradingRuleEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuDowngradingRuleEntity> manuDowngradingRuleEntitys);

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
        Task<ManuDowngradingRuleEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuDowngradingRuleEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuDowngradingRuleQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuDowngradingRuleEntity>> GetManuDowngradingRuleEntitiesAsync(ManuDowngradingRuleQuery manuDowngradingRuleQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuDowngradingRulePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuDowngradingRuleEntity>> GetPagedInfoAsync(ManuDowngradingRulePagedQuery manuDowngradingRulePagedQuery);
        #endregion

        /// <summary>
        /// 获取最大排序序号的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuDowngradingRuleEntity> GetMaxSerialNumberAsync(long siteId);

        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuDowngradingRuleEntity> GetByCodeAsync(ManuDowngradingRuleCodeQuery query);
        Task<IEnumerable<ManuDowngradingRuleEntity>> GetByCodesAsync(ManuDowngradingRuleCodesQuery query);

        /// <summary>
        /// 批量更新序号
        /// </summary>
        /// <param name="manuDowngradingRuleEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateSerialNumbersAsync(List<ManuDowngradingRuleEntity> manuDowngradingRuleEntitys);
    }
}
