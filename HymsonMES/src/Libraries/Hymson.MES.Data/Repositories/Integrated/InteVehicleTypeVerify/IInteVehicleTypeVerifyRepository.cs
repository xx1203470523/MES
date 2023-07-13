/*
 *creator: Karl
 *
 *describe: 载具类型验证仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-13 03:15:22
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具类型验证仓储接口
    /// </summary>
    public interface IInteVehicleTypeVerifyRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehicleTypeVerifyEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteVehicleTypeVerifyEntity inteVehicleTypeVerifyEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehicleTypeVerifyEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<InteVehicleTypeVerifyEntity> inteVehicleTypeVerifyEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehicleTypeVerifyEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteVehicleTypeVerifyEntity inteVehicleTypeVerifyEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteVehicleTypeVerifyEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<InteVehicleTypeVerifyEntity> inteVehicleTypeVerifyEntitys);

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
        Task<InteVehicleTypeVerifyEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleTypeVerifyEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteVehicleTypeVerifyQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleTypeVerifyEntity>> GetInteVehicleTypeVerifyEntitiesAsync(InteVehicleTypeVerifyQuery inteVehicleTypeVerifyQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteVehicleTypeVerifyPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteVehicleTypeVerifyEntity>> GetPagedInfoAsync(InteVehicleTypeVerifyPagedQuery inteVehicleTypeVerifyPagedQuery);
        #endregion

        /// <summary>
        /// 批量删除 (硬删除) 根据 VehicleTypeId
        /// </summary>
        /// <param name="vehicleTypeIds"></param>
        /// <returns></returns>
        Task<int> DeletesTrueByVehicleTypeIdAsync(long[] vehicleTypeIds);

        /// <summary>
        /// 查询List 获取载具类型验证 根据vehicleTypeId查询
        /// </summary>
        /// <param name="vehicleTypeIds"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleTypeVerifyEntity>> GetInteVehicleTypeVerifyEntitiesByVehicleTyleIdAsync(long[] vehicleTypeIds);
    }
}
