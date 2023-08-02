using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 消息组 验证
    /// </summary>
    internal class InteMessageGroupSaveValidator: AbstractValidator<InteMessageGroupSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteMessageGroupSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
