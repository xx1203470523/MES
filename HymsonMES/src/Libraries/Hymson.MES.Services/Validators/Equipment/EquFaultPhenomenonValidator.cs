using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 验证（设备组）
    /// </summary>
    internal class EquFaultPhenomenonValidator : AbstractValidator<EquFaultPhenomenonSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquFaultPhenomenonValidator()
        {
            RuleFor(x => x.FaultPhenomenonCode).NotEmpty().WithErrorCode(ErrorCode.MES12901);
            RuleFor(x => x.FaultPhenomenonCode).MaximumLength(50).WithErrorCode(ErrorCode.MES10109);
            RuleFor(x => x.FaultPhenomenonName).NotEmpty().WithErrorCode(ErrorCode.MES12902);
            RuleFor(x => x.FaultPhenomenonName).MaximumLength(50).WithErrorCode(ErrorCode.MES10110);
            RuleFor(x => x.EquipmentGroupId).Must(it => it != 0).WithErrorCode(ErrorCode.MES12904);
            RuleFor(x => x.UseStatus).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12907));
            RuleFor(x => x.UseStatus).Must(it => Enum.IsDefined(typeof(SysDataStatusEnum), it)).WithErrorCode(ErrorCode.MES12906);

        }
    }
}
