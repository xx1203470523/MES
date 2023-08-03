using FluentValidation;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 工艺设备组 验证
    /// </summary>
    internal class ProcProcessEquipmentGroupSaveValidator: AbstractValidator<ProcProcessEquipmentGroupSaveDto>
    {
        public ProcProcessEquipmentGroupSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
