using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// IQC检验水平 验证
    /// </summary>
    internal class QualIqcLevelSaveValidator : AbstractValidator<QualIqcLevelSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualIqcLevelSaveValidator()
        {
            RuleFor(x => x).NotNull().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Type).Must(it => Enum.IsDefined(typeof(QCMaterialTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES19410));
            RuleFor(x => x.Level).Must(it => Enum.IsDefined(typeof(InspectionLevelEnum), it)).WithErrorCode(nameof(ErrorCode.MES19411));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(DisableOrEnableEnum), it)).WithErrorCode(nameof(ErrorCode.MES19412));
            RuleFor(x => x.AcceptanceLevel).NotNull().GreaterThanOrEqualTo(0).WithErrorCode(nameof(ErrorCode.MES19413));
        }
    }

    /// <summary>
    /// IQC检验水平 验证
    /// </summary>
    internal class QualIqcLevelDetailSaveValidator : AbstractValidator<QualIqcLevelDetailDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualIqcLevelDetailSaveValidator()
        {
            RuleFor(x => x).NotNull().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Type).Must(it => it.HasValue && Enum.IsDefined(typeof(IQCInspectionTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES19414));
            RuleFor(x => x.VerificationLevel).Must(it => it.HasValue && Enum.IsDefined(typeof(VerificationLevelEnum), it)).WithErrorCode(nameof(ErrorCode.MES19415));
            RuleFor(x => x.AcceptanceLevel).NotNull().GreaterThanOrEqualTo(0).WithErrorCode(nameof(ErrorCode.MES19416));
        }
    }

}
