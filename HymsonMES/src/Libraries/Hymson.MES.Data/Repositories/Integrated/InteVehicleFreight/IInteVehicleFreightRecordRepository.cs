/*
 *creator: Karl
 *
 *describe: 载具装载记录仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-24 04:45:45
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具装载记录仓储接口
    /// </summary>
    public interface IInteVehicleFreightRecordRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehicleFreightRecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteVehicleFreightRecordEntity inteVehicleFreightRecordEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehicleFreightRecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<InteVehicleFreightRecordEntity> inteVehicleFreightRecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehicleFreightRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteVehicleFreightRecordEntity inteVehicleFreightRecordEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteVehicleFreightRecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<InteVehicleFreightRecordEntity> inteVehicleFreightRecordEntitys);

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
        Task<InteVehicleFreightRecordEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleFreightRecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteVehicleFreightRecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleFreightRecordEntity>> GetInteVehicleFreightRecordEntitiesAsync(InteVehicleFreightRecordQuery inteVehicleFreightRecordQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteVehicleFreightRecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteVehicleFreightRecordEntity>> GetPagedInfoAsync(InteVehicleFreightRecordPagedQuery inteVehicleFreightRecordPagedQuery);
        #endregion
    }
}
