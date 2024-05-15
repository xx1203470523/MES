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
        public async Task<List<ProcSortRuleDto>> GetSortRuleDetailAsync(ProcSortRuleDetailEquQuery param)
        {
            var dbList = await _procSortingRuleRepository.GetSortRuleDetailAsync(param);
            if(dbList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45160));
            }

            List<ProcSortRuleDto> resultList = new List<ProcSortRuleDto>();

            var groupNameList = dbList.Select(m => m.Grade).Distinct().ToList();
            foreach (var groupItem in groupNameList) //遍历每个挡位，每个挡位下会有多个规则
            {
                ProcSortRuleDto ruleDto = new ProcSortRuleDto();
                ruleDto.Grade = groupItem;

                var groupIdList = dbList.Where(m => m.Grade == groupItem).Select(m => m.GradeId).Distinct().ToList();
                foreach (var groupId in groupIdList) //遍历每个挡位的规则（一个规则可能包含多个条件）
                {
                    var groupDbList = dbList.Where(m => m.GradeId == groupId).ToList();
                    List<ProcSortRuleDetailEquDto> groupList = new List<ProcSortRuleDetailEquDto>();
                    foreach (var item in groupDbList)
                    {
                        ProcSortRuleDetailEquDto model = new ProcSortRuleDetailEquDto();
                        model.SortRuleCode = item.Code;
                        model.SortRuleName = item.Name;
                        model.ParameterCode = item.ParameterCode;
                        model.ParameterName = item.ParameterName;
                        model.MinValue = item.MinValue;
                        model.MinContainingType = item.MinContainingType;
                        model.MaxValue = item.MaxValue;
                        model.MaxContainingType = item.MaxContainingType;
                        model.Rating = item.Rating;
                        model.Serial = item.Serial;
                        model.ParameterValue = item.ParameterValue;
                        model.Grade = item.Grade;
                        model.ProcedureCode = item.ProcedureCode;

                        groupList.Add(model);
                    }
                    ruleDto.RuleList.Add(groupList);
                }
                resultList.Add(ruleDto);
            }
            return resultList;
        }
    }
}
