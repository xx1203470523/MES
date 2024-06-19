/*
 *creator: Karl
 *
 *describe: 设备维修记录仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquRepairOrder;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquRepairOrder
{
    /// <summary>
    /// 设备维修记录仓储接口
    /// </summary>
    public interface IEquRepairOrderRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equRepairOrderEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquRepairOrderEntity equRepairOrderEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equRepairOrderEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquRepairOrderEntity> equRepairOrderEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equRepairOrderEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquRepairOrderEntity equRepairOrderEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equRepairOrderEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquRepairOrderEntity> equRepairOrderEntitys);

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
        Task<EquRepairOrderEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquRepairOrderEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equRepairOrderQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquRepairOrderEntity>> GetEquRepairOrderEntitiesAsync(EquRepairOrderQuery equRepairOrderQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equRepairOrderPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquRepairOrderEntity>> GetPagedInfoAsync(EquRepairOrderPagedQuery equRepairOrderPagedQuery);
        #endregion
    }
}
