/*
 *creator: Karl
 *
 *describe: 工单信息表    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-03-20 10:07:17
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
    /// 工单信息表 更新 验证
    /// </summary>
    internal class PlanWorkOrderCreateValidator: AbstractValidator<PlanWorkOrderCreateDto>
    {
        public PlanWorkOrderCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// 工单信息表 修改 验证
    /// </summary>
    internal class PlanWorkOrderModifyValidator : AbstractValidator<PlanWorkOrderModifyDto>
    {
        public PlanWorkOrderModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
