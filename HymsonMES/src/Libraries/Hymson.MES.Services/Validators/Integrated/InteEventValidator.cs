using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 事件维护 验证
    /// </summary>
    internal class InteEventSaveValidator: AbstractValidator<InteEventSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteEventSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
