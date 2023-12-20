using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 验证器（设备故障原因）
    /// </summary>
    internal class EquFaultReasonValidator : AbstractValidator<EquFaultReasonSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquFaultReasonValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(ErrorCode.MES10113);
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(ErrorCode.MES10109);
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(ErrorCode.MES10116);
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(ErrorCode.MES10110);
            //RuleFor(x => x.EquipmentGroupId).Must(it => it != 0).WithErrorCode(ErrorCode.MES12904);

        }
    }
}
