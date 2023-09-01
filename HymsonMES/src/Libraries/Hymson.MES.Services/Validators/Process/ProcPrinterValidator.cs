using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 资源配置打印机表 更新 验证
    /// </summary>
    internal class ProcPrinterCreateValidator : AbstractValidator<ProcPrinterDto>
    {
        public ProcPrinterCreateValidator()
        {
            RuleFor(x => x.PrintName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10346));
            RuleFor(x => x.PrintName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10110));
            RuleFor(x => x.PrintIp).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10347));
            RuleFor(x => x.PrintIp).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10371));

        }
    }

    /// <summary>
    /// 资源配置打印机表 修改 验证
    /// </summary>
    internal class ProcPrinterModifyValidator : AbstractValidator<ProcPrinterUpdateDto>
    {
        public ProcPrinterModifyValidator()
        {
            RuleFor(x => x.PrintName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10346));
            RuleFor(x => x.PrintName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10110));

        }
    }
}
