using FluentValidation;
using Hymson.MES.Core.Constants;
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
            RuleFor(x => x.FaultPhenomenonName).NotEmpty().WithErrorCode(ErrorCode.MES12902);
            RuleFor(x => x.EquipmentGroupId).NotEmpty().Must(it => it != 0).WithErrorCode(ErrorCode.MES12904);
        }
    }
}
