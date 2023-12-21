using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment.EquFaultPhenomenon
{
    /// <summary>
    /// service接口（设备故障现象）
    /// </summary>
    public interface IEquFaultPhenomenonService
    {
        /// <summary>
        /// 添加（设备故障现象）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquFaultPhenomenonSaveDto parm);

        /// <summary>
        /// 更新（设备故障现象）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquFaultPhenomenonSaveDto parm);

        /// <summary>
        /// 删除（设备故障现象）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 查询列表（设备故障现象）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<PagedInfo<EquFaultPhenomenonDto>> GetPagedListAsync(EquFaultPhenomenonPagedQueryDto parm);

        /// <summary>
        /// 查询详情（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquFaultPhenomenonDto> GetDetailAsync(long id);

        /// <summary>
        /// 查询已经分配设备故障原因
        /// </summary>
        /// <param name="equFaultPhenomenonQueryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<EquFaultReasonDto>> GetEquFaultReasonListAsync(EquFaultPhenomenonQueryDto equFaultPhenomenonQueryDto);


        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="statusDto"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(ChangeStatusDto statusDto);

    }
}
