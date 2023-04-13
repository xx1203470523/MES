using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 验证（设备注册）
    /// </summary>
    internal class EquEquipmentValidator : AbstractValidator<EquEquipmentSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquEquipmentValidator()
        {
            RuleFor(x => x.EquipmentCode).NotEmpty().WithErrorCode(ErrorCode.MES12601);
            RuleFor(x => x.EquipmentName).NotEmpty().WithErrorCode(ErrorCode.MES12602);
        }
    }
}
