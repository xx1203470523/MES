using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.EquSpotcheckTemplate
{
    /// <summary>
    /// 工具新增验证
    /// </summary>
    internal class EquToolingManageCreateValidator : AbstractValidator<AddEquToolingManageDto>
    {
        public EquToolingManageCreateValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(ErrorCode.MES13505);
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(ErrorCode.MES13506);
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(ErrorCode.MES13507);
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(ErrorCode.MES13508);
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(ErrorCode.MES13509);
            RuleFor(x => x.ToolsId).NotEmpty().WithErrorCode(ErrorCode.MES13510);
            RuleFor(x => x.RatedLife).NotEmpty().WithErrorCode(ErrorCode.MES13511);
            RuleFor(x => x.IsCalibrated).IsInEnum().WithErrorCode(ErrorCode.MES13512);
            RuleFor(x => x).Must((x)=>{
                if (x.IsCalibrated == YesOrNoEnum.Yes)
                    return x.CalibrationCycle.HasValue;
                else
                    return true;
            }).WithErrorCode(ErrorCode.MES13513);
            RuleFor(x => x).Must((x) => {
                if (x.IsCalibrated == YesOrNoEnum.Yes)
                    return x.CalibrationCycleUnit.HasValue;
                else
                    return true;
            }).WithErrorCode(ErrorCode.MES13514);
            RuleFor(x => x.CalibrationCycle).PrecisionScale(8, 0, false).WithErrorCode(ErrorCode.MES13513);
            RuleFor(x => x).Must((x) => {
                if (x.IsCalibrated == YesOrNoEnum.Yes)
                    return x.LastVerificationTime.HasValue;
                else
                    return true;
            }).WithErrorCode(ErrorCode.MES13515);
        }
    }

    /// <summary>
    ///  工具更新验证
    /// </summary>
    internal class EquToolingManageModifyValidator : AbstractValidator<EquToolingManageModifyDto>
    {
        public EquToolingManageModifyValidator()
        {
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(ErrorCode.MES13509);
            RuleFor(x => x.ToolsId).NotEmpty().WithErrorCode(ErrorCode.MES13510);
            RuleFor(x => x.RatedLife).NotEmpty().WithErrorCode(ErrorCode.MES13511);
            RuleFor(x => x.IsCalibrated).IsInEnum().WithErrorCode(ErrorCode.MES13512);
            RuleFor(x => x).Must((x) => {
                if (x.IsCalibrated == YesOrNoEnum.Yes)
                    return x.CalibrationCycle.HasValue;
                else
                    return true;
            }).WithErrorCode(ErrorCode.MES13513);
            RuleFor(x => x.CalibrationCycle).PrecisionScale(8, 0, false).WithErrorCode(ErrorCode.MES13513);
            RuleFor(x => x).Must((x) => {
                if (x.IsCalibrated == YesOrNoEnum.Yes)
                    return x.CalibrationCycleUnit.HasValue;
                else
                    return true;
            }).WithErrorCode(ErrorCode.MES13514);
        }
    }

    /// <summary>
    ///  工具更新验证
    /// </summary>
    internal class EquToolingManageeExcelValidator : AbstractValidator<EquToolingManageExcelDto>
    {
        public EquToolingManageeExcelValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(ErrorCode.MES13505);
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(ErrorCode.MES13506);
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(ErrorCode.MES13507);
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(ErrorCode.MES13508);
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(ErrorCode.MES13509);
            RuleFor(x => x.ToolTypeCode).NotEmpty().WithErrorCode(ErrorCode.MES13510);
            RuleFor(x => x.RatedLife).NotEmpty().WithErrorCode(ErrorCode.MES13511);
            RuleFor(x => x.IsCalibrated).IsInEnum().WithErrorCode(ErrorCode.MES13512);
            RuleFor(x => x).Must((x) => {
                if (x.IsCalibrated == YesOrNoEnum.Yes)
                    return x.CalibrationCycle.HasValue;
                else
                    return true;
            }).WithErrorCode(ErrorCode.MES13513);

            RuleFor(x => x.CalibrationCycle).PrecisionScale(8, 0, false).WithErrorCode(ErrorCode.MES13518);
            RuleFor(x => x).Must((x) => {
                if (x.IsCalibrated == YesOrNoEnum.Yes)
                    return x.CalibrationCycleUnit.HasValue;
                else
                    return true;
            }).WithErrorCode(ErrorCode.MES13514);

            RuleFor(x => x).Must((x) => {
                if (x.IsCalibrated == YesOrNoEnum.Yes)
                    return x.LastVerificationTime.HasValue;
                else
                    return true;
            }).WithErrorCode(ErrorCode.MES13515);
        }
    }
}
