/*
 *creator: Karl
 *
 *describe: 物料维护    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-07 11:16:51
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
    /// 物料维护 验证
    /// </summary>
    internal class ProcMaterialValidator: AbstractValidator<ProcMaterialDto>
    {
        public ProcMaterialValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
