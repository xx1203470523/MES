using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 配方维护 验证
    /// </summary>
    internal class ProcFormulaSaveValidator: AbstractValidator<ProcFormulaSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ProcFormulaSaveValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15704));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15705));
            RuleFor(x => x.Version).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15706));
            RuleFor(x => x.MaterialId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15707));
            RuleFor(x => x.ProcedureId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15708));
            RuleFor(x => x.EquipmentGroupId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15709));
            RuleFor(x => x.FormulaOperationGroupId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15710));

            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES15712));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES15713));
            RuleFor(x => x.Version).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES15714));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES15715));
        }
    }

    internal class ProcFormulaDetailsDtoValidator : AbstractValidator<ProcFormulaDetailsDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ProcFormulaDetailsDtoValidator()
        {
            RuleFor(x => x.Serial).Must(x=>x>0).WithErrorCode(nameof(ErrorCode.MES15719));
            RuleFor(x => x.FormulaOperationId).Must(x => x > 0).WithErrorCode(nameof(ErrorCode.MES15720));
            RuleFor(x => x.Setvalue).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15721));
            RuleFor(x => x.Unit).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15722));

            RuleFor(x => x.Setvalue).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES15716));
            RuleFor(x => x.Unit).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES15717));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES15718));
        }
    }

}
