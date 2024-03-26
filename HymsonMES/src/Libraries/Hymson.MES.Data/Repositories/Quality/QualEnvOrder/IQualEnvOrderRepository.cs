/*
 *creator: Karl
 *
 *describe: 环境检验单仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:53
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.QualEnvOrder;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.QualEnvOrder
{
    /// <summary>
    /// 环境检验单仓储接口
    /// </summary>
    public interface IQualEnvOrderRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualEnvOrderEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(QualEnvOrderEntity qualEnvOrderEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="qualEnvOrderEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<QualEnvOrderEntity> qualEnvOrderEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="qualEnvOrderEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(QualEnvOrderEntity qualEnvOrderEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="qualEnvOrderEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<QualEnvOrderEntity> qualEnvOrderEntitys);

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
        Task<QualEnvOrderEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<QualEnvOrderEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="qualEnvOrderQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<QualEnvOrderEntity>> GetQualEnvOrderEntitiesAsync(QualEnvOrderQuery qualEnvOrderQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="qualEnvOrderPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<QualEnvOrderEntity>> GetPagedInfoAsync(QualEnvOrderPagedQuery qualEnvOrderPagedQuery);
        #endregion
    }
}
