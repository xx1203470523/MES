/*
 *creator: Karl
 *
 *describe: 物料组维护表    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-10 03:54:07
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
    /// 物料组维护表 更新 验证
    /// </summary>
    internal class ProcMaterialGroupCreateValidator: AbstractValidator<ProcMaterialGroupCreateDto>
    {
        public ProcMaterialGroupCreateValidator()
        {
            // TODO SiteId  RuleFor(x => x.SiteCode).NotEmpty().WithErrorCode(ErrorCode.MES10203);
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// 物料组维护表 修改 验证
    /// </summary>
    internal class ProcMaterialGroupModifyValidator : AbstractValidator<ProcMaterialGroupModifyDto>
    {
        public ProcMaterialGroupModifyValidator()
        {
            // TODO SiteId RuleFor(x => x.SiteCode).NotEmpty().WithErrorCode(ErrorCode.MES10203);
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
