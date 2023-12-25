using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 配方操作 验证
    /// </summary>
    internal class ProcFormulaOperationSaveValidator : AbstractValidator<ProcFormulaOperationSaveDto>
    {
        public ProcFormulaOperationSaveValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10113));
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10109));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10116));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10110));
            RuleFor(x => x.Version).NotEmpty().Must(it => it != "").WithErrorCode(nameof(ErrorCode.MES10618));

            RuleFor(x => x.Type).Must(it => Enum.IsDefined(typeof(FormulaOperationTypeEnum), it)).WithErrorCode("配方操作类型不合法");
        }
    }

}
