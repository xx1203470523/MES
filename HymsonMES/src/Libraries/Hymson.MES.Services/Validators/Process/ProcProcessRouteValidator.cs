/*
 *creator: Karl
 *
 *describe: 工艺路线表    验证规则 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:07:11
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 工艺路线表 更新 验证
    /// </summary>
    internal class ProcProcessRouteCreateValidator: AbstractValidator<ProcProcessRouteCreateDto>
    {
        public ProcProcessRouteCreateValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(ErrorCode.MES10432);
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(ErrorCode.MES10433);
            RuleFor(x => x.Version).NotEmpty().WithErrorCode(ErrorCode.MES10434);
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// 工艺路线表 修改 验证
    /// </summary>
    internal class ProcProcessRouteModifyValidator : AbstractValidator<ProcProcessRouteModifyDto>
    {
        public ProcProcessRouteModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
