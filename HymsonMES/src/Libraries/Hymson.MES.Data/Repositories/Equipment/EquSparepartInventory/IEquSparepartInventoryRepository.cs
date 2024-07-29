/*
 *creator: Karl
 *
 *describe: 备件库存仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:15:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquSparepartInventory;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquSparepartInventory
{
    /// <summary>
    /// 备件库存仓储接口
    /// </summary>
    public interface IEquSparepartInventoryRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSparepartInventoryEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquSparepartInventoryEntity equSparepartInventoryEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equSparepartInventoryEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquSparepartInventoryEntity> equSparepartInventoryEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSparepartInventoryEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquSparepartInventoryEntity equSparepartInventoryEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equSparepartInventoryEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquSparepartInventoryEntity> equSparepartInventoryEntitys);

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
        Task<EquSparepartInventoryEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据SparepartId获取数据
        /// </summary>
        /// <param name="SparepartId"></param>
        /// <returns></returns>
        Task<EquSparepartInventoryEntity> GetBySparepartIdAsync(EquSparepartInventoryQuery equSparepartInventoryQuery);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSparepartInventoryEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equSparepartInventoryQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSparepartInventoryEntity>> GetEquSparepartInventoryEntitiesAsync(EquSparepartInventoryQuery equSparepartInventoryQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSparepartInventoryPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSparepartInventoryPageView>> GetPagedInfoAsync(EquSparepartInventoryPagedQuery equSparepartInventoryPagedQuery);

        #endregion
    }
}
