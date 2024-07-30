using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 跨工序时间管控 更新 验证
    /// </summary>
    internal class ProcProcedureTimeControlCreateValidator : AbstractValidator<ProcProcedureTimeControlCreateDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ProcProcedureTimeControlCreateValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(ErrorCode.MES10113);
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(ErrorCode.MES10109);
            RuleFor(x => x.Code).Matches("^[^`~!@#$%^&*()+<>?:\\\"\\'{},.\\/;\\[\\]\\\\]+$").WithErrorCode(nameof(ErrorCode.MES10132));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(ErrorCode.MES10116);
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(ErrorCode.MES10110);
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES13904));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(SysDataStatusEnum), it)).WithErrorCode(ErrorCode.MES10230);
        }
    }

    /// <summary>
    /// 跨工序时间管控 修改 验证
    /// </summary>
    internal class ProcProcedureTimeControlModifyValidator : AbstractValidator<ProcProcedureTimeControlModifyDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ProcProcedureTimeControlModifyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(ErrorCode.MES10116);
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(ErrorCode.MES10110);
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES13904));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(SysDataStatusEnum), it)).WithErrorCode(ErrorCode.MES10230);
        }
    }
}
