using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 产品工序时间仓储接口
    /// </summary>
    public interface IProcProductTimecontrolRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProductTimecontrolEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcProductTimecontrolEntity procProductTimecontrolEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProductTimecontrolEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcProductTimecontrolEntity> procProductTimecontrolEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProductTimecontrolEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcProductTimecontrolEntity procProductTimecontrolEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procProductTimecontrolEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcProductTimecontrolEntity> procProductTimecontrolEntitys);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);

        /// <summary>
        /// 删除  
        /// 最好使用批量删除，可以设置更新人和更新时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProductTimecontrolEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProductTimecontrolEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procProductTimecontrolQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProductTimecontrolEntity>> GetProcProductTimecontrolEntitiesAsync(ProcProductTimecontrolQuery procProductTimecontrolQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProductTimecontrolView>> GetPagedInfoAsync(ProcProductTimecontrolPagedQuery pagedQuery);
        #endregion
    }
}
