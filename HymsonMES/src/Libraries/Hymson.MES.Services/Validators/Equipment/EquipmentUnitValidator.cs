using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 
    /// </summary>
    internal class EquipmentUnitValidator : AbstractValidator<EquipmentUnitDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquipmentUnitValidator()
        {
            RuleFor(x => x.UnitCode).NotEmpty().WithErrorCode("11").WithMessage("11");
            RuleFor(x => x.UnitName).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
