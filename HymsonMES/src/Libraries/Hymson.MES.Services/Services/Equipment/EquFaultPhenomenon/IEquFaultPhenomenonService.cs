using Hymson.Infrastructure;
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
        Task<int> CreateAsync(EquFaultPhenomenonCreateDto parm);

        /// <summary>
        /// 更新（设备故障现象）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquFaultPhenomenonModifyDto parm);

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


    }
}
