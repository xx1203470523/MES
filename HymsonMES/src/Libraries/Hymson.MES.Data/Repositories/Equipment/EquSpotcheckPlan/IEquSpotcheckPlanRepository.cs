/*
 *creator: Karl
 *
 *describe: 设备点检计划仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-20 04:05:45
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquSpotcheckPlan
{
    /// <summary>
    /// 设备点检计划仓储接口
    /// </summary>
    public interface IEquSpotcheckPlanRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSpotcheckPlanEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquSpotcheckPlanEntity equSpotcheckPlanEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equSpotcheckPlanEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquSpotcheckPlanEntity> equSpotcheckPlanEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSpotcheckPlanEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquSpotcheckPlanEntity equSpotcheckPlanEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equSpotcheckPlanEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquSpotcheckPlanEntity> equSpotcheckPlanEntitys);

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
        Task<EquSpotcheckPlanEntity> GetByIdAsync(long id);


        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        Task<EquSpotcheckPlanEntity> GetByCodeAsync(EquSpotcheckPlanQuery param);


        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSpotcheckPlanEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equSpotcheckPlanQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSpotcheckPlanEntity>> GetEquSpotcheckPlanEntitiesAsync(EquSpotcheckPlanQuery equSpotcheckPlanQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSpotcheckPlanPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSpotcheckPlanEntity>> GetPagedInfoAsync(EquSpotcheckPlanPagedQuery equSpotcheckPlanPagedQuery);
        #endregion
    }
}
