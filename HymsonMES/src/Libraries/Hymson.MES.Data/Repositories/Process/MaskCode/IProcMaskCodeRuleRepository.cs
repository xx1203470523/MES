using Hymson.MES.Core.Domain.Process;

namespace Hymson.MES.Data.Repositories.Process.MaskCode
{
    /// <summary>
    /// 仓储接口（掩码规则维护）
    /// </summary>
    public interface IProcMaskCodeRuleRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ProcMaskCodeRuleEntity> entities);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maskCodeId"></param>
        /// <returns></returns>
        Task<int> ClearByMaskCodeId(long maskCodeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maskCodeId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaskCodeRuleEntity>> GetByMaskCodeIdAsync(long maskCodeId);
    }
}
