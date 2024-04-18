/*
 *creator: Karl
 *
 *describe: 载具装载仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-18 09:52:16
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具装载仓储接口
    /// </summary>
    public interface IInteVehicleFreightRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehicleFreightEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteVehicleFreightEntity inteVehicleFreightEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehicleFreightEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<InteVehicleFreightEntity> inteVehicleFreightEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehicleFreightEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteVehicleFreightEntity inteVehicleFreightEntity);
        Task<int> UpdateQtyAsync(InteVehicleFreightEntity inteVehicleFreightEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteVehicleFreightEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<InteVehicleFreightEntity> inteVehicleFreightEntitys);

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
        Task<InteVehicleFreightEntity> GetByIdAsync(long id);
       
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleFreightEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteVehicleFreightQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleFreightEntity>> GetInteVehicleFreightEntitiesAsync(InteVehicleFreightQuery inteVehicleFreightQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteVehicleFreightPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteVehicleFreightEntity>> GetPagedInfoAsync(InteVehicleFreightPagedQuery inteVehicleFreightPagedQuery);
        #endregion

        /// <summary>
        ///获取数据 根据vehicleId
        /// </summary>
        /// <param name="vehicleIds"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleFreightEntity>> GetByVehicleIdsAsync(long[] vehicleIds);

        /// <summary>
        /// 批量删除 (硬删除) 根据 VehicleId
        /// </summary>
        /// <param name="vehicleIds"></param>
        /// <returns></returns>
        Task<int> DeletesTrueByVehicleIdsAsync(long[] vehicleIds);

        #region 顷刻

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        Task<int> UpdateQtyByLocationAsync(List<InteVehicleFreightEntity> entityList);

        #endregion
    }
}
