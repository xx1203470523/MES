using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 物料维护 更新 验证
    /// </summary>
    internal class ProcMaterialCreateValidator : AbstractValidator<ProcMaterialCreateDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ProcMaterialCreateValidator()
        {
            RuleFor(x => x.MaterialCode).NotEmpty().WithErrorCode(ErrorCode.MES10214);
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
            RuleFor(x => x.MaterialName).NotEmpty().WithErrorCode(ErrorCode.MES10215);
            RuleFor(x => x.MaterialCode).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES10223));
            RuleFor(x => x.MaterialName).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES10224));
            RuleFor(x => x.SerialNumber).NotNull().WithErrorCode(ErrorCode.MES10226);
        }
    }

    /// <summary>
    /// 物料维护 修改 验证
    /// </summary>
    internal class ProcMaterialModifyValidator : AbstractValidator<ProcMaterialModifyDto>
    {
        public ProcMaterialModifyValidator()
        {
            RuleFor(x => x.MaterialName).NotEmpty().WithErrorCode(ErrorCode.MES10215);
            RuleFor(x => x.MaterialName).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES10224));
            // TODO SiteId RuleFor(x => x.SiteCode).NotEmpty().WithErrorCode(ErrorCode.MES10203);// 判断是否有获取到站点码
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
            RuleFor(x => x.SerialNumber).NotNull().WithErrorCode(ErrorCode.MES10226);
        }
    }
}
