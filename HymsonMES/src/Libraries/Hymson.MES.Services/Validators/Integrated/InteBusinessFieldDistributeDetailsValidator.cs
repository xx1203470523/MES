using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 字段分配管理详情 验证
    /// </summary>
    internal class InteBusinessFieldDistributeDetailsSaveValidator: AbstractValidator<InteBusinessFieldDistributeDetailsSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteBusinessFieldDistributeDetailsSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
