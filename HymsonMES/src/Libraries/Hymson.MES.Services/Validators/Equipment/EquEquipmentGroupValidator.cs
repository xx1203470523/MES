using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 验证（设备组）
    /// </summary>
    internal class EquEquipmentGroupValidator : AbstractValidator<EquEquipmentGroupSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquEquipmentGroupValidator()
        {
            RuleFor(x => x.EquipmentGroupCode).NotEmpty().WithErrorCode(ErrorCode.MES12701);
            RuleFor(x => x.EquipmentGroupName).NotEmpty().WithErrorCode(ErrorCode.MES12702);
        }
    }
}
