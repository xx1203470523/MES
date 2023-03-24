/*
 *creator: Karl
 *
 *describe: 条码接收    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
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
    internal class PlanSfcReceiveCreateValidator : AbstractValidator<PlanSfcReceiveCreateDto>
    {
        public PlanSfcReceiveCreateValidator()
        {
            RuleFor(x => x.ReceiveType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16101));
            RuleFor(x => x.WorkOrderId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16102));
            //RuleFor(x => x.relevanceWorkOrderId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16103));
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16104));
        }
    }

    /// <summary>
    /// 条码接收 修改 验证
    /// </summary>
    internal class PlanSfcReceiveModifyValidator : AbstractValidator<PlanSfcReceiveModifyDto>
    {
        public PlanSfcReceiveModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
