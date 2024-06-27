/*
 *creator: Karl
 *
 *describe: 生产领料单明细仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-04 02:34:40
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder
{
    /// <summary>
    /// 生产领料单明细仓储接口
    /// </summary>
    public interface IManuRequistionOrderDetailRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuRequistionOrderDetailEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuRequistionOrderDetailEntity manuRequistionOrderDetailEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuRequistionOrderDetailEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuRequistionOrderDetailEntity> manuRequistionOrderDetailEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuRequistionOrderDetailEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuRequistionOrderDetailEntity manuRequistionOrderDetailEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuRequistionOrderDetailEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuRequistionOrderDetailEntity> manuRequistionOrderDetailEntitys);

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
        Task<ManuRequistionOrderDetailEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuRequistionOrderDetailEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuRequistionOrderDetailQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuRequistionOrderDetailEntity>> GetManuRequistionOrderDetailEntitiesAsync(ManuRequistionOrderDetailQuery manuRequistionOrderDetailQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuRequistionOrderDetailPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuRequistionOrderDetailEntity>> GetPagedInfoAsync(ManuRequistionOrderDetailPagedQuery manuRequistionOrderDetailPagedQuery);
        #endregion
    }
}
