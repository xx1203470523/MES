using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProcSortingRule.View;
using Hymson.MES.EquipmentServices.Dtos.Qkny.ProcSortingRule;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.ProcSortingRule
{
    /// <summary>
    /// 分选规则
    /// </summary>
    public class ProcSortingRuleService : IProcSortingRuleService
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly IProcSortingRuleRepository _procSortingRuleRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcSortingRuleService(IProcSortingRuleRepository procSortingRuleRepository)
        {
            _procSortingRuleRepository = procSortingRuleRepository;
        }

        /// <summary>
        /// 获取分选规则
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortRuleDetailEquDto>> GetSortRuleDetailAsync(ProcSortRuleDetailEquQuery param)
        {
            var dbList = await _procSortingRuleRepository.GetSortRuleDetailAsync(param);
            if(dbList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45160));
            }
            List<ProcSortRuleDetailEquDto> resultList = new List<ProcSortRuleDetailEquDto>();
            foreach(var item in  dbList)
            {
                ProcSortRuleDetailEquDto model = new ProcSortRuleDetailEquDto();
                model.SortRuleCode = item.Code;
                model.SortRuleName = item.Name;
                model.ParameterValue = item.ParameterValue;
                model.MinValue = item.MinValue;
                model.MinContainingType = item.MinContainingType;
                model.MaxValue = item.MaxValue;
                model.MaxContainingType = item.MaxContainingType;
                model.Rating = item.Rating;
                model.Serial = item.Serial;
                model.ParameterValue = item.ParameterValue;
                model.Grade = item.Grade;
                model.ProcedureCode = item.ProcedureCode;

                resultList.Add(model);
            }

            return resultList;
        }
    }
}
