/*
 *creator: Karl
 *
 *describe: ESOP    验证规则 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-11-02 02:39:53
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// ESOP 更新 验证
    /// </summary>
    internal class ProcEsopCreateValidator: AbstractValidator<ProcEsopCreateDto>
    {
        public ProcEsopCreateValidator()
        {
            
        }
    }

    /// <summary>
    /// ESOP 修改 验证
    /// </summary>
    internal class ProcEsopModifyValidator : AbstractValidator<ProcEsopModifyDto>
    {
        public ProcEsopModifyValidator()
        {
            
        }
    }

    /// <summary>
    /// ESOP获取Job验证
    /// </summary>
    internal class ProcEsopGetJobValidator : AbstractValidator<ProcEsopGetJobQueryDto>
    {
        public ProcEsopGetJobValidator()
        {
            RuleFor(a => a.ProcedureId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16335));
            RuleFor(a => a.ResourceId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16334));
        }
    }
}
