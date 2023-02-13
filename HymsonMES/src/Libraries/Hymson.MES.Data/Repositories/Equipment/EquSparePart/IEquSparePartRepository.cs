using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart.Query;

namespace Hymson.MES.Data.Repositories.Equipment.EquSparePart
{
    /// <summary>
    /// 备件注册仓储接口
    /// </summary>
    public interface IEquSparePartRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquSparePartEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquSparePartEntity entity);

        /// <summary>
        /// 批量修改备件的备件类型
        /// </summary>
        /// <param name="sparePartTypeId"></param>
        /// <param name="sparePartIds"></param>
        /// <returns></returns>
        Task<int> UpdateSparePartTypeIdAsync(long sparePartTypeId, IEnumerable<long> sparePartIds);

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
        Task<EquSparePartEntity> GetByIdAsync(long id);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSparePartEntity>> GetEquSparePartEntitiesAsync(EquSparePartQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSparePartEntity>> GetPagedInfoAsync(EquSparePartPagedQuery pagedQuery);

    }
}
