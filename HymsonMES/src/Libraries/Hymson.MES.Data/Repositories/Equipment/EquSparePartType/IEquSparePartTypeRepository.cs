using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquSparePartType.Query;

namespace Hymson.MES.Data.Repositories.Equipment.EquSparePartType
{
    /// <summary>
    /// 备件类型仓储接口
    /// </summary>
    public interface IEquSparePartTypeRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSparePartTypeEntity"></param>
        /// <returns></returns>
        Task InsertAsync(EquSparePartTypeEntity equSparePartTypeEntity);
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSparePartTypeEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquSparePartTypeEntity equSparePartTypeEntity);
        
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
        Task<EquSparePartTypeEntity> GetByIdAsync(long id);
        
        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equSparePartTypeQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSparePartTypeEntity>> GetEquSparePartTypeEntitiesAsync(EquSparePartTypeQuery equSparePartTypeQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSparePartTypePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSparePartTypeEntity>> GetPagedInfoAsync(EquSparePartTypePagedQuery equSparePartTypePagedQuery);
    }
}
