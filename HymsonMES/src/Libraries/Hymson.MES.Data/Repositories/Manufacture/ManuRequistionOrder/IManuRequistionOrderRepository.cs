/*
 *creator: Karl
 *
 *describe: 生产领料单仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-04 02:34:15
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder
{
    /// <summary>
    /// 生产领料单仓储接口
    /// </summary>
    public interface IManuRequistionOrderRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuRequistionOrderEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuRequistionOrderEntity manuRequistionOrderEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuRequistionOrderEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuRequistionOrderEntity> manuRequistionOrderEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuRequistionOrderEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuRequistionOrderEntity manuRequistionOrderEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuRequistionOrderEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuRequistionOrderEntity> manuRequistionOrderEntitys);

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
        Task<ManuRequistionOrderEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuRequistionOrderEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuRequistionOrderQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuRequistionOrderEntity>> GetManuRequistionOrderEntitiesAsync(ManuRequistionQueryByWorkOrders manuRequistionOrderQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuRequistionOrderPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuRequistionOrderEntity>> GetPagedInfoAsync(ManuRequistionOrderPagedQuery manuRequistionOrderPagedQuery);

        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuRequistionOrderEntity> GetByCodeAsync(ManuRequistionOrderQuery query);
        #endregion
    }
}
