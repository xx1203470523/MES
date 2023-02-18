using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquConsumableType.Query;

namespace Hymson.MES.Data.Repositories.Equipment.EquConsumableType
{
    /// <summary>
    /// 仓储接口（工装类型）
    /// </summary>
    public interface IEquConsumableTypeRepository
    {
        /// <summary>
        /// 新增（工装类型）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquConsumableTypeEntity entity);

        /// <summary>
        /// 更新（工装类型）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquConsumableTypeEntity entity);

        /// <summary>
        /// 删除（工装类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除（工装类型）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 根据ID获取数据（工装类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquConsumableTypeEntity> GetByIdAsync(long id);

        /// <summary>
        /// 获取List（工装类型）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquConsumableTypeEntity>> GetEntitiesAsync(EquConsumableTypeQuery query);

        /// <summary>
        /// 分页查询（工装类型）
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquConsumableTypeEntity>> GetPagedInfoAsync(EquConsumableTypePagedQuery pagedQuery);
        
    }
}
