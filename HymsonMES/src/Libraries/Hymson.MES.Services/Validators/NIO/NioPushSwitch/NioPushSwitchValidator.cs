using FluentValidation;
using Hymson.MES.Services.Dtos.NioPushSwitch;

namespace Hymson.MES.Services.Validators.NioPushSwitch
{
    /// <summary>
    /// 蔚来推送开关 验证
    /// </summary>
    internal class NioPushSwitchSaveValidator: AbstractValidator<NioPushSwitchSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public NioPushSwitchSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
