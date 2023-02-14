/*
 *creator: Karl
 *
 *describe: 作业表    验证规则 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-02-14 04:32:34
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 作业表 更新 验证
    /// </summary>
    internal class InteJobCreateValidator: AbstractValidator<InteJobCreateDto>
    {
        public InteJobCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// 作业表 修改 验证
    /// </summary>
    internal class InteJobModifyValidator : AbstractValidator<InteJobModifyDto>
    {
        public InteJobModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
