using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备故障原因表仓储接口
    /// </summary>
    public interface IEquFaultReasonRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equFaultReasonEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquFaultReasonEntity equFaultReasonEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equFaultReasonEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<EquFaultReasonEntity> equFaultReasonEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equFaultReasonEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquFaultReasonEntity equFaultReasonEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equFaultReasonEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<EquFaultReasonEntity> equFaultReasonEntitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquFaultReasonEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquFaultReasonEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquFaultReasonEntity> GetByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="EquFaultReasonQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquFaultReasonEntity>> GetListAsync(EquFaultReasonQuery EquFaultReasonQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="EquFaultReasonPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquFaultReasonEntity>> GetPagedInfoAsync(EquFaultReasonPagedQuery EquFaultReasonPagedQuery);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);

    }
}
