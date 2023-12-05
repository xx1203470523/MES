using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Process;

namespace Hymson.MES.CoreServices.Services.Common.MasterData
{
    /// <summary>
    /// 主数据公用类
    /// @author wangkeming
    /// @date 2023-06-06
    /// </summary>
    public partial class MasterDataService
    {
        /// <summary>
        /// 获取分选规则列表
        /// </summary>
        /// <param name="unqualifiedIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleDetailEntity>> GetSortingRuleDetailByIdAsync(long sortingRuleId)
        {
            return await _sortingRuleDetailRepository.GetSortingRuleDetailByIdAsync(sortingRuleId);
        }

        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProductTimecontrolEntity>> GetProductTimecontrolAsync(long siteId, long procedureId, long productId)
        {
            var procProducts = await _procProductTimecontrolRepository.GetProcProductTimecontrolEntitiesAsync(new ProcProductTimecontrolQuery
            {
                SiteId = siteId,
                ProcedureId = procedureId,
                ProductId = productId,
                Status = SysDataStatusEnum.Enable
            });
            return procProducts;
        }

    }
}
