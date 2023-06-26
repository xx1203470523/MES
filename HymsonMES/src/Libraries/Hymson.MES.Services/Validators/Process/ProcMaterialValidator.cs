using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
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
            RuleFor(x => x.MaterialName).NotEmpty().WithErrorCode(ErrorCode.MES10215);
            RuleFor(x => x.MaterialCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10223));
            RuleFor(x => x.MaterialName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10224));
            RuleFor(x => x.SerialNumber).Must(it => it != null && Enum.IsDefined(typeof(MaterialSerialNumberEnum), it)).WithErrorCode(ErrorCode.MES10227);
            RuleFor(x => x.Batch).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES10228));
            RuleFor(x => x.BuyType).Must(it => it != null && Enum.IsDefined(typeof(MaterialBuyTypeEnum), it)).WithErrorCode(ErrorCode.MES10229);
            RuleFor(x => x.Status).Must(it => it != null && Enum.IsDefined(typeof(SysDataStatusEnum), it)).WithErrorCode(ErrorCode.MES10230);
            RuleFor(x => x.Version).NotEmpty().WithErrorCode(ErrorCode.MES10231);

        }
    }

    /// <summary>
    /// 物料维护 修改 验证
    /// </summary>
    internal class ProcMaterialModifyValidator : AbstractValidator<ProcMaterialModifyDto>
    {
        public ProcMaterialModifyValidator()
        {
            RuleFor(x => x.MaterialCode).NotEmpty().WithErrorCode(ErrorCode.MES10214);
            RuleFor(x => x.MaterialName).NotEmpty().WithErrorCode(ErrorCode.MES10215);
            RuleFor(x => x.MaterialCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10223));
            RuleFor(x => x.MaterialName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10224));
            RuleFor(x => x.SerialNumber).Must(it => it != null && Enum.IsDefined(typeof(MaterialSerialNumberEnum), it)).WithErrorCode(ErrorCode.MES10227);
            RuleFor(x => x.Batch).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES10228));
            RuleFor(x => x.BuyType).Must(it => it != null && Enum.IsDefined(typeof(MaterialBuyTypeEnum), it)).WithErrorCode(ErrorCode.MES10229);
            RuleFor(x => x.Status).Must(it => it != null && Enum.IsDefined(typeof(SysDataStatusEnum), it)).WithErrorCode(ErrorCode.MES10230);
            RuleFor(x => x.Version).NotEmpty().WithErrorCode(ErrorCode.MES10231);
        }
    }
}
