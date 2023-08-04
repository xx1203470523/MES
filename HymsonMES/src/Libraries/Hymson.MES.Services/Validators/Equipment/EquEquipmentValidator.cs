using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 验证（设备注册）
    /// </summary>
    public class EquEquipmentValidator : AbstractValidator<EquEquipmentSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquEquipmentValidator()
        {
            RuleFor(x => x.EquipmentCode).NotEmpty().WithErrorCode(ErrorCode.MES12601);
            RuleFor(x => x.EquipmentCode).MaximumLength(50).WithErrorCode(ErrorCode.MES10109);
            RuleFor(x => x.EquipmentName).NotEmpty().WithErrorCode(ErrorCode.MES12602);
            RuleFor(x => x.EquipmentName).MaximumLength(50).WithErrorCode(ErrorCode.MES10110);
            RuleFor(x => x.UseStatus).Must(it => Enum.IsDefined(typeof(EquipmentUseStatusEnum), it)).WithErrorCode(ErrorCode.MES12605);
            RuleFor(x => x.Location).NotEmpty().WithErrorCode(ErrorCode.MES12606);
        }
    }
}
