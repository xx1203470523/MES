/*
 *creator: Karl
 *
 *describe: 标准参数关联类型表    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-13 05:06:17
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 标准参数关联类型表 更新 验证
    /// </summary>
    internal class ProcParameterLinkTypeCreateValidator : AbstractValidator<ProcParameterLinkTypeCreateDto>
    {
        public ProcParameterLinkTypeCreateValidator()
        {
            RuleFor(x => x.ParameterType).NotNull().Must(x => x != null && Enum.IsDefined(typeof(ParameterTypeEnum), x)).WithErrorCode(nameof(ErrorCode.MES10513));
            RuleFor(x => x.Parameters).NotNull().Must(x => x != null && !x.Where(it => it <= 0).Any()).WithErrorCode(nameof(ErrorCode.MES10514));

            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// 标准参数关联类型表 修改 验证
    /// </summary>
    internal class ProcParameterLinkTypeModifyValidator : AbstractValidator<ProcParameterLinkTypeModifyDto>
    {
        public ProcParameterLinkTypeModifyValidator()
        {
            RuleFor(x => x.ParameterType).Must(x => Enum.IsDefined(typeof(ParameterTypeEnum), x)).WithErrorCode(nameof(ErrorCode.MES10513));
            RuleFor(x => x.Parameters).NotEmpty().Must(x => !x.Where(it => it <= 0).Any()).WithErrorCode(nameof(ErrorCode.MES10514));
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
