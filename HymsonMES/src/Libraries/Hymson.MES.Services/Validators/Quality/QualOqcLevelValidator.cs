using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// OQC检验水平 验证
    /// </summary>
    internal class QualOqcLevelSaveValidator : AbstractValidator<QualOqcLevelSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualOqcLevelSaveValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Type).Must(it => Enum.IsDefined(typeof(QCMaterialTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES19410));
            RuleFor(x => x.Level).Must(it => Enum.IsDefined(typeof(InspectionLevelEnum), it)).WithErrorCode(nameof(ErrorCode.MES19411));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(DisableOrEnableEnum), it)).WithErrorCode(nameof(ErrorCode.MES19412));
            RuleFor(x => x.AcceptanceLevel).NotNull().GreaterThanOrEqualTo(0).WithErrorCode(nameof(ErrorCode.MES19413));
        }
    }

    /// <summary>
    /// OQC检验水平 验证
    /// </summary>
    internal class QualOqcLevelDetailSaveValidator : AbstractValidator<QualOqcLevelDetailDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualOqcLevelDetailSaveValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Type).Must(it => it.HasValue && Enum.IsDefined(typeof(OQCInspectionTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES19414));
            RuleFor(x => x.VerificationLevel).Must(it => it.HasValue && Enum.IsDefined(typeof(VerificationLevelEnum), it)).WithErrorCode(nameof(ErrorCode.MES19415));
            RuleFor(x => x.AcceptanceLevel).NotNull().GreaterThanOrEqualTo(0).WithErrorCode(nameof(ErrorCode.MES19416));
        }
    }

}
