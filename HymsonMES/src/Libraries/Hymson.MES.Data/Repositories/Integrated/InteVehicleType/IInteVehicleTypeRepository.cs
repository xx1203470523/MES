/*
 *creator: Karl
 *
 *describe: 载具类型维护仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-12 10:37:17
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具类型维护仓储接口
    /// </summary>
    public interface IInteVehicleTypeRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehicleTypeEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteVehicleTypeEntity inteVehicleTypeEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehicleTypeEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<InteVehicleTypeEntity> inteVehicleTypeEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehicleTypeEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteVehicleTypeEntity inteVehicleTypeEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteVehicleTypeEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<InteVehicleTypeEntity> inteVehicleTypeEntitys);

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
        /// 根据Code获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<InteVehicleTypeEntity> GetByCodeAsync(InteVehicleTypeCodeQuery query);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteVehicleTypeEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleTypeEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteVehicleTypeQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleTypeEntity>> GetInteVehicleTypeEntitiesAsync(InteVehicleTypeQuery inteVehicleTypeQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteVehicleTypePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteVehicleTypeEntity>> GetPagedInfoAsync(InteVehicleTypePagedQuery inteVehicleTypePagedQuery);

        /// <summary>
        /// 根据编码查询数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleTypeEntity>> GetByCodesAsync(InteVehicleTypeNameQuery query);
        #endregion
    }
}
