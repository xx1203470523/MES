/*
 *creator: Karl
 *
 *describe: 标准参数表    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-13 02:50:20
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
    /// 标准参数表 更新 验证
    /// </summary>
    internal class ProcParameterCreateValidator: AbstractValidator<ProcParameterCreateDto>
    {
        public ProcParameterCreateValidator()
        {
            RuleFor(x => x.SiteCode).NotEmpty().WithErrorCode(ErrorCode.MES10501);
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// 标准参数表 修改 验证
    /// </summary>
    internal class ProcParameterModifyValidator : AbstractValidator<ProcParameterModifyDto>
    {
        public ProcParameterModifyValidator()
        {
            RuleFor(x => x.SiteCode).NotEmpty().WithErrorCode(ErrorCode.MES10501);
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
