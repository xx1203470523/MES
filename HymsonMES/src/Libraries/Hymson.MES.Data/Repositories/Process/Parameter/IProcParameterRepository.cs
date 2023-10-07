using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.Parameter.Query;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 标准参数表仓储接口
    /// </summary>
    public interface IProcParameterRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procParameterEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcParameterEntity procParameterEntity);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procParameterEntities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ProcParameterEntity> procParameterEntities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procParameterEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcParameterEntity procParameterEntity);

        /// <summary>
        /// 删除
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
        Task<ProcParameterEntity> GetByIdAsync(long id);

        /// <summary>
        /// 更具编码获取参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcParameterEntity>> GetByCodesAsync(ProcParametersByCodeQuery param);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcParameterEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procParameterQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcParameterEntity>> GetProcParameterEntitiesAsync(ProcParameterQuery procParameterQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procParameterPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcParameterEntity>> GetPagedListAsync(ProcParameterPagedQuery procParameterPagedQuery);

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcParameterEntity>> GetAllAsync(long siteId);
    }
}
