using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 字段分配管理 验证
    /// </summary>
    internal class InteBusinessFieldDistributeSaveValidator: AbstractValidator<InteBusinessFieldDistributeSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteBusinessFieldDistributeSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
