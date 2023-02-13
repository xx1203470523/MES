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
        /// 分页查询
        /// </summary>
        /// <param name="procResourceEquipmentBindPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceEquipmentBindView>> GetPagedInfoAsync(ProcResourceEquipmentBindPagedQuery procResourceEquipmentBindPagedQuery);

        /// <summary>
        /// 根据资源id和设备Id查询数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEquipmentBindEntity>> GetByResourceIdAsync(ProcResourceEquipmentBindQuery query);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procResourceEquipmentBinds"></param>
        /// <returns></returns>
        Task InsertRangeAsync(List<ProcResourceEquipmentBindEntity> procResourceEquipmentBinds);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceEquipmentBinds"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(List<ProcResourceEquipmentBindEntity> procResourceEquipmentBinds);
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesRangeAsync(long[] idsArr);
    }
}
