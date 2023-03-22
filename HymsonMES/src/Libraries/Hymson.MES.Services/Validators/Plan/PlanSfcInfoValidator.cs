/*
 *creator: Karl
 *
 *describe: 条码接收    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Plan
{
    /// <summary>
    /// 条码接收 更新 验证
    /// </summary>
    internal class PlanSfcInfoCreateValidator: AbstractValidator<PlanSfcInfoCreateDto>
    {
        public PlanSfcInfoCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// 条码接收 修改 验证
    /// </summary>
    internal class PlanSfcInfoModifyValidator : AbstractValidator<PlanSfcInfoModifyDto>
    {
        public PlanSfcInfoModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
