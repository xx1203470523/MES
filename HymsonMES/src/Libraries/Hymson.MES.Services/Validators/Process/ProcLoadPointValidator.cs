/*
 *creator: Karl
 *
 *describe: 上料点表    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 上料点表 更新 验证
    /// </summary>
    internal class ProcLoadPointCreateValidator: AbstractValidator<ProcLoadPointCreateDto>
    {
        public ProcLoadPointCreateValidator()
        {
            //RuleFor(x => x.LinkMaterials.ToString()).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// 上料点表 修改 验证
    /// </summary>
    internal class ProcLoadPointModifyValidator : AbstractValidator<ProcLoadPointModifyDto>
    {
        public ProcLoadPointModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
