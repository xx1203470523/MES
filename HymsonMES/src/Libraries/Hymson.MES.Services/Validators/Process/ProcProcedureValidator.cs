/*
 *creator: Karl
 *
 *describe: 工序表    验证规则 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-02-13 09:06:05
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Process;
namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 工序表 更新 验证
    /// </summary>
    internal class ProcProcedureCreateValidator : AbstractValidator<ProcProcedureCreateDto>
    {
        public ProcProcedureCreateValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10401));
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10402));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10403));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10404));
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10409));
            RuleFor(x => x.Type).Must(x => Enum.IsDefined(typeof(ProcedureTypeEnum), x)).WithErrorCode(nameof(ErrorCode.MES10407));

        }
    }

    /// <summary>
    /// 工序表 修改 验证
    /// </summary>
    internal class ProcProcedureModifyValidator : AbstractValidator<ProcProcedureModifyDto>
    {
        public ProcProcedureModifyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10403));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10404));
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10409));
            RuleFor(x => x.Type).Must(x => Enum.IsDefined(typeof(ProcedureTypeEnum), x)).WithErrorCode(nameof(ErrorCode.MES10407));

        }
    }
}
