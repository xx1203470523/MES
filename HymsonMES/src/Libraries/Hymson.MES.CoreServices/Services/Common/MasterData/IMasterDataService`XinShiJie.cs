using Hymson.MES.Core.Domain.Process;

namespace Hymson.MES.CoreServices.Services.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IMasterDataService
    {
        /// <summary>
        /// 获取分选规则列表
        /// </summary>
        /// <param name="sortingRuleId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcSortingRuleDetailEntity>> GetSortingRuleDetailByIdAsync(long sortingRuleId);

        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProductTimecontrolEntity>> GetProductTimecontrolAsync(long siteId, long procedureId, long productId);

    }
}
