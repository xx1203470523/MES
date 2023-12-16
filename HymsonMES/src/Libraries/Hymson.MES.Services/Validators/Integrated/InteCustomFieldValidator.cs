using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 自定义字段 验证
    /// </summary>
    internal class InteCustomFieldSaveValidator: AbstractValidator<InteCustomFieldSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteCustomFieldSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
