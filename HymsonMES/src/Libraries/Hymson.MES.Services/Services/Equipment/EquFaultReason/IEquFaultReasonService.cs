using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 设备故障原因表 service接口
    /// </summary>
    public interface IEquFaultReasonService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquFaultReasonSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquFaultReasonSaveDto saveDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquFaultReasonDto>> GetPageListAsync(EquFaultReasonPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquFaultReasonDto> QueryByIdAsync(long id);

        /// <summary>
        /// 获取解决措施（可被引用）
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SelectOptionDto>> QueryReasonsAsync();

        /// <summary>
        /// 根据ID获取关联解决措施
        /// </summary>
        /// <param name="phenomenonId"></param>
        /// <returns></returns>
        Task<IEnumerable<SelectOptionDto>> QueryReasonsByMainIdAsync(long phenomenonId);


        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="statusDto"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(ChangeStatusDto statusDto);

    }
}
