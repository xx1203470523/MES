using Hymson.MES.Data.Repositories.Process.ProcSortingRule.View;
using Hymson.MES.Data.Repositories.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.MES.EquipmentServices.Dtos.Qkny.ProcSortingRule;

namespace Hymson.MES.EquipmentServices.Services.Qkny.ProcSortingRule
{
    /// <summary>
    /// 分选规则
    /// </summary>
    public interface IProcSortingRuleService
    {
        /// <summary>
        /// 获取分选规则
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcSortRuleDetailEquDto>> GetSortRuleDetailAsync(ProcSortRuleDetailEquQuery param);
    }
}
