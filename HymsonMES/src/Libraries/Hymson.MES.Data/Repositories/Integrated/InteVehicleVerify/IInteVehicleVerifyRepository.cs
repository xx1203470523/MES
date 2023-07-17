/*
 *creator: Karl
 *
 *describe: 载具校验仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-17 09:34:37
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具校验仓储接口
    /// </summary>
    public interface IInteVehicleVerifyRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehicleVerifyEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteVehicleVerifyEntity inteVehicleVerifyEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehicleVerifyEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<InteVehicleVerifyEntity> inteVehicleVerifyEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehicleVerifyEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteVehicleVerifyEntity inteVehicleVerifyEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteVehicleVerifyEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<InteVehicleVerifyEntity> inteVehicleVerifyEntitys);

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
        Task<InteVehicleVerifyEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleVerifyEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteVehicleVerifyQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleVerifyEntity>> GetInteVehicleVerifyEntitiesAsync(InteVehicleVerifyQuery inteVehicleVerifyQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteVehicleVerifyPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteVehicleVerifyEntity>> GetPagedInfoAsync(InteVehicleVerifyPagedQuery inteVehicleVerifyPagedQuery);
        #endregion

        /// <summary>
        /// 批量删除 (硬删除) 根据 VehicleId
        /// </summary>
        /// <param name="vehicleIds"></param>
        /// <returns></returns>
        Task<int> DeletesTrueByVehicleIdsAsync(long[] vehicleIds);

        /// <summary>
        ///获取数据 根据vehicleId
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        Task<InteVehicleVerifyEntity> GetByVehicleIdAsync(long vehicleId);
    }
}
