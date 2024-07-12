using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 工具类型管理 验证
    /// </summary>
    internal class EquToolsTypeSaveValidator : AbstractValidator<EquToolsTypeSaveDto>
    {
        /// <summary>
        /// 验证
        /// </summary>
        public EquToolsTypeSaveValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(ErrorCode.MES13505);
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(ErrorCode.MES10115);
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(ErrorCode.MES13507);
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(ErrorCode.MES10117);
            //RuleFor(x => x.Status).NotEmpty().WithErrorCode(ErrorCode.MES13509);
            RuleFor(x => x.Status).Must(x => Enum.IsDefined(typeof(DisableOrEnableEnum), x)).WithErrorCode(nameof(ErrorCode.MES13012));
            // RuleFor(x => x.RatedLife).NotEmpty().WithErrorCode(ErrorCode.MES13511);
            //RuleFor(x => x.IsCalibrated).IsInEnum().WithErrorCode(ErrorCode.MES13512);
            //RuleFor(x => x).Must((x) =>
            //{
            //    if (x.IsCalibrated == YesOrNoEnum.Yes)
            //        return x.CalibrationCycle.HasValue;
            //    else
            //        return true;
            //}).WithErrorCode(ErrorCode.MES13513);
            //RuleFor(x => x).Must((x) =>
            //{
            //    if (x.IsCalibrated == YesOrNoEnum.Yes)
            //        return x.CalibrationCycleUnit.HasValue;
            //    else
            //        return true;
            //}).WithErrorCode(ErrorCode.MES13514);
            //RuleFor(x => x).Must((x) =>
            //{
            //    if (x.IsCalibrated == YesOrNoEnum.Yes)
            //        return x.LastVerificationTime.HasValue;
            //    else
            //        return true;
            //}).WithErrorCode(ErrorCode.MES13515);
        }
    }

}
