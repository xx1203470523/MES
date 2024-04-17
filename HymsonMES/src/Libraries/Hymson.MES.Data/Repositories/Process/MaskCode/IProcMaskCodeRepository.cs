using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process.MaskCode.Query;


namespace Hymson.MES.Data.Repositories.Process.MaskCode
{
    /// <summary>
    /// 仓储接口（掩码维护）
    /// </summary>
    public interface IProcMaskCodeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcMaskCodeEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcMaskCodeEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcMaskCodeEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcMaskCodeEntity> GetByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcMaskCodeEntity>> GetPagedListAsync(ProcMaskCodePagedQuery pagedQuery);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaskCodeEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 根据编码获取掩码信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaskCodeEntity>> GetByCodesAsync(ProcMaskCodesByCodeQuery param);

    }
}
