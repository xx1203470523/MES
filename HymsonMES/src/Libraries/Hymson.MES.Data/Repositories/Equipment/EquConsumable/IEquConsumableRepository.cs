using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquConsumable.Command;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart.Query;

namespace Hymson.MES.Data.Repositories.Equipment.EquSparePart
{
    /// <summary>
    /// 仓储接口（工装注册）
    /// </summary>
    public interface IEquConsumableRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquConsumableEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquConsumableEntity entity);

        /// <summary>
        /// 批量修改备件的备件类型
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateConsumableTypeIdAsync(UpdateConsumableTypeIdCommand command);

        /// <summary>
        /// 清空备件的指定备件类型
        /// </summary>
        /// <param name="sparePartTypeId"></param>
        /// <returns></returns>
        Task<int> ClearConsumableTypeIdAsync(long sparePartTypeId);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquConsumableEntity> GetByIdAsync(long id);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquConsumableEntity>> GetEntitiesAsync(EquConsumableQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquConsumableEntity>> GetPagedInfoAsync(EquConsumablePagedQuery pagedQuery);

    }
}
