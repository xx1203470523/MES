using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquFaultPhenomenon.Query;

namespace Hymson.MES.Data.Repositories.Equipment.EquFaultPhenomenon
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
        /// 判断是否存在（编码）
        /// </summary>
        /// <param name="faultPhenomenonCode"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(string faultPhenomenonCode);

        /// <summary>
        /// 根据ID获取数据（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquFaultPhenomenonEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据ID获取数据（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquFaultPhenomenonView> GetViewByIdAsync(long id);

        /// <summary>
        /// 分页查询（设备故障现象）
        /// </summary>
        /// <param name="equFaultPhenomenonPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquFaultPhenomenonView>> GetPagedInfoAsync(EquFaultPhenomenonPagedQuery equFaultPhenomenonPagedQuery);
    }
}
