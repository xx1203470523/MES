/*
 *creator: Karl
 *
 *describe: 资源设备绑定表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 11:20:47
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源设备绑定表仓储接口
    /// </summary>
    public interface IProcResourceEquipmentBindRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procResourceEquipmentBindEntity"></param>
        /// <returns></returns>
        Task InsertAsync(ProcResourceEquipmentBindEntity procResourceEquipmentBindEntity);
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceEquipmentBindEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcResourceEquipmentBindEntity procResourceEquipmentBindEntity);
        
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
        Task<ProcResourceEquipmentBindEntity> GetByIdAsync(long id);
        
        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procResourceEquipmentBindQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEquipmentBindEntity>> GetProcResourceEquipmentBindEntitiesAsync(ProcResourceEquipmentBindPagedQuery procResourceEquipmentBindQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procResourceEquipmentBindPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceEquipmentBindView>> GetPagedInfoAsync(ProcResourceEquipmentBindPagedQuery procResourceEquipmentBindPagedQuery);
    }
}
