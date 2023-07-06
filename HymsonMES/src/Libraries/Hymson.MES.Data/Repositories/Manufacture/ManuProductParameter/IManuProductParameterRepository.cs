using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 生产过程参数仓储接口
    /// </summary>
    public interface IManuProductParameterRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuProductParameterEntity entity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<ManuProductParameterEntity> entities);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(EquipmentIdQuery query);


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuProductParameterEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuProductParameterEntity manuProductParameterEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuProductParameterEntities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuProductParameterEntity> manuProductParameterEntities);

        /// <summary>
        /// 查询ManuProductParameter
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductParameterEntity>> GetManuProductParameterAsync(ManuProductParameterQuery query);

        /// <summary>
        /// 参数分页查询
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuProductParameterView>> GetManuProductParameterPagedInfoAsync(ManuProductParameterPagedQuery queryParam);
    }
}
