/*
 *creator: Karl
 *
 *describe: 仓库标签模板    验证规则 | 代码由框架生成  
 *builder:  wxk
 *build datetime: 2023-03-09 02:51:26
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
    /// 仓库标签模板 更新 验证
    /// </summary>
    internal class ProcLabelTemplateCreateValidator : AbstractValidator<ProcLabelTemplateCreateDto>
    {
        public ProcLabelTemplateCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10343));
            RuleFor(x => x.Path).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10344));
            RuleFor(x => x.Path).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES10370));

            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10342));

 
        }
    }

    /// <summary>
    /// 仓库标签模板 修改 验证
    /// </summary>
    internal class ProcLabelTemplateModifyValidator : AbstractValidator<ProcLabelTemplateModifyDto>
    {
        public ProcLabelTemplateModifyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10343));
            RuleFor(x => x.Path).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10344));
            RuleFor(x => x.Path).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES10370));

            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10342));

        }
    }
}
