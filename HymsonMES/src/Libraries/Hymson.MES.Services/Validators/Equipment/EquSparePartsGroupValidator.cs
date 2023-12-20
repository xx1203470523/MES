using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 备件类型 验证
    /// </summary>
    internal class EquSparePartsGroupSaveValidator: AbstractValidator<EquSparePartsGroupSaveDto>
    {
        public EquSparePartsGroupSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
