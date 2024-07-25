/*
 *creator: Karl
 *
 *describe: 马威FQC检验仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-07-24 03:09:40
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.QualFqcInspectionMaval;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.QualFqcInspectionMaval
{
    /// <summary>
    /// 马威FQC检验仓储接口
    /// </summary>
    public interface IQualFqcInspectionMavalRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualFqcInspectionMavalEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(QualFqcInspectionMavalEntity qualFqcInspectionMavalEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="qualFqcInspectionMavalEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<QualFqcInspectionMavalEntity> qualFqcInspectionMavalEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="qualFqcInspectionMavalEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(QualFqcInspectionMavalEntity qualFqcInspectionMavalEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="qualFqcInspectionMavalEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<QualFqcInspectionMavalEntity> qualFqcInspectionMavalEntitys);

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
        Task<QualFqcInspectionMavalEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualFqcInspectionMavalEntity> GetBySFCAsync(QualFqcInspectionMavalQuery param);


        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<QualFqcInspectionMavalEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="qualFqcInspectionMavalQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<QualFqcInspectionMavalEntity>> GetQualFqcInspectionMavalEntitiesAsync(QualFqcInspectionMavalQuery qualFqcInspectionMavalQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="qualFqcInspectionMavalPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<QualFqcInspectionMavalEntity>> GetPagedInfoAsync(QualFqcInspectionMavalPagedQuery qualFqcInspectionMavalPagedQuery);
        #endregion
    }
}
