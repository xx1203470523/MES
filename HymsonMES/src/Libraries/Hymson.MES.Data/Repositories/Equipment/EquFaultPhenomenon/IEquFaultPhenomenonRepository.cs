using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.Query;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储接口（设备故障现象）
    /// </summary>
    public interface IEquFaultPhenomenonRepository
    {
        /// <summary>
        /// 新增（设备故障现象）
        /// </summary>
        /// <param name="equFaultPhenomenonEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquFaultPhenomenonEntity equFaultPhenomenonEntity);

        /// <summary>
        /// 新增（设备故障现象和原因关系）
        /// </summary>
        /// <param name="equFaultReasonPhenomenonEntities"></param>
        /// <returns></returns>
        Task<int> InsertFaultReasonAsync(IEnumerable<EquFaultPhenomenonReasonRelationEntity> equFaultReasonPhenomenonEntities);

        /// <summary>
        /// 更新（设备故障现象）
        /// </summary>
        /// <param name="equFaultPhenomenonEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquFaultPhenomenonEntity equFaultPhenomenonEntity);

        /// <summary>
        /// 删除（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除（设备故障现象）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 根据ID获取数据（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquFaultPhenomenonEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquFaultPhenomenonEntity> GetByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 分页查询（设备故障现象）
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquFaultPhenomenonEntity>> GetPagedInfoAsync(EquFaultPhenomenonPagedQuery pagedQuery);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquFaultPhenomenonEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);

        /// <summary>
        /// 获取已经分配设备故障原因
        /// </summary>
        /// <param name="equFaultPhenomenonQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquFaultPhenomenonReasonRelationEntity>> GetEquFaultReasonListAsync(EquFaultPhenomenonQuery equFaultPhenomenonQuery);

        /// <summary>
        /// 删除设备故障原因关系（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteEquFaultReasonPhenomenonRelationsAsync(DeleteCommand command);

    }
}
