/*
 *creator: Karl
 *
 *describe: 维修结果仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:58:46
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquRepairResult;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquRepairResult
{
    /// <summary>
    /// 维修结果仓储接口
    /// </summary>
    public interface IEquRepairResultRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equRepairResultEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquRepairResultEntity equRepairResultEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equRepairResultEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquRepairResultEntity> equRepairResultEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equRepairResultEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquRepairResultEntity equRepairResultEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equRepairResultEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquRepairResultEntity> equRepairResultEntitys);

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
        Task<EquRepairResultEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquRepairResultEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equRepairResultQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquRepairResultEntity>> GetEquRepairResultEntitiesAsync(EquRepairResultQuery equRepairResultQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equRepairResultPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquRepairResultEntity>> GetPagedInfoAsync(EquRepairResultPagedQuery equRepairResultPagedQuery);
        #endregion
    }
}
