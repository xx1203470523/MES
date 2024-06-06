using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// 产品检验参数表 验证
    /// </summary>
    internal class ProcProductParameterGroupValidator : AbstractValidator<ProcProductParameterGroupSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ProcProductParameterGroupValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10113));
            RuleFor(x => x.Code).Must(x => !x.Any(x => char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES10114));
            RuleFor(x => x.Code).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES10115));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10116));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES10117));
            RuleFor(x => x.Version).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10118));
            RuleFor(x => x.Version).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES10119));
            RuleFor(x => x.Version).Must(x => !x.Any(x => char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES10122));

            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES10121));

            RuleFor(x => x.MaterialId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10518));
            RuleFor(x => x.ProcedureId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10519));
        }
    }


    internal class ProcProductParameterGroupImportValidator : AbstractValidator<ProcEquipmentGroupParamDetailParamImportDto>
    {
        public ProcProductParameterGroupImportValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10113));
            RuleFor(x => x.Code).Must(x => !x.Any(x => Char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES10114));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10116));
            RuleFor(x => x.Code).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES10115));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES10117));
        }
    }

}
