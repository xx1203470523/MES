/*
 *creator: Karl
 *
 *describe: 环境检验单检验明细仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:43
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.QualEnvOrderDetail;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.QualEnvOrderDetail
{
    /// <summary>
    /// 环境检验单检验明细仓储接口
    /// </summary>
    public interface IQualEnvOrderDetailRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualEnvOrderDetailEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(QualEnvOrderDetailEntity qualEnvOrderDetailEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="qualEnvOrderDetailEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<QualEnvOrderDetailEntity> qualEnvOrderDetailEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="qualEnvOrderDetailEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(QualEnvOrderDetailEntity qualEnvOrderDetailEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="qualEnvOrderDetailEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<QualEnvOrderDetailEntity> qualEnvOrderDetailEntitys);

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
        Task<QualEnvOrderDetailEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<QualEnvOrderDetailEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="qualEnvOrderDetailQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<QualEnvOrderDetailEntity>> GetQualEnvOrderDetailEntitiesAsync(QualEnvOrderDetailQuery qualEnvOrderDetailQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="qualEnvOrderDetailPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<QualEnvOrderDetailEntity>> GetPagedInfoAsync(QualEnvOrderDetailPagedQuery qualEnvOrderDetailPagedQuery);
        #endregion

        #region
        /// <summary>
        /// 批量更新(执行检验)
        /// </summary>
        /// <param name="qualEnvOrderDetailEntitys"></param>
        /// <returns></returns> 
        Task<int> UpdatesExecAsync(List<QualEnvOrderDetailEntity> qualEnvOrderDetailEntitys);
        /// <summary>
        /// 根据检验单ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<QualEnvOrderDetailEntity>> GetByEnvOrderIdAsync(long envOrderId);

        /// <summary>
        /// 根据ID获取快照明细
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<QualEnvParameterGroupDetailSnapshootEntity>> GetGroupDetailSnapshootByIdsAsync(long[] ids);
        #endregion
    }
}
