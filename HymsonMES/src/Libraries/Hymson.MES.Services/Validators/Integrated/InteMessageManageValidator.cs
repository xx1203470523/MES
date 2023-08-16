using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 消息管理 验证
    /// </summary>
    internal class InteMessageManageSaveValidator: AbstractValidator<InteMessageManageSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteMessageManageSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
