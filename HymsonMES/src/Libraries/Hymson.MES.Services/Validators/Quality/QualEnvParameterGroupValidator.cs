using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// 环境检验参数表 验证
    /// </summary>
    internal class QualEnvParameterGroupValidator : AbstractValidator<QualEnvParameterGroupSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualEnvParameterGroupValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10113));
            RuleFor(x => x.Code).Must(x => x.Any(x => char.IsWhiteSpace(x)) == false).WithErrorCode(nameof(ErrorCode.MES10114));
            RuleFor(x => x.Code).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES10115));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10116));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES10117));
            RuleFor(x => x.Version).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10118));
            RuleFor(x => x.Version).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES10119));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(SysDataStatusEnum), it)).WithErrorCode(nameof(ErrorCode.MES10120));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES10121));

            RuleFor(x => x.WorkCenterId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10517));
            RuleFor(x => x.ProcedureId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10519));
        }
    }

}
