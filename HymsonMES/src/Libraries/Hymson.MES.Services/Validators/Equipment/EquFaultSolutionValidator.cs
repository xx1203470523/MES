using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 验证器（设备故障解决措施）
    /// </summary>
    internal class EquFaultSolutionValidator : AbstractValidator<EquFaultSolutionSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquFaultSolutionValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(ErrorCode.MES10113);
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(ErrorCode.MES10109);
            RuleFor(x => x.Name).Matches("^[a-zA-Z0-9]+$").WithErrorCode(nameof(ErrorCode.MES10131));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(ErrorCode.MES10116);
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(ErrorCode.MES10110);

        }
    }
}
