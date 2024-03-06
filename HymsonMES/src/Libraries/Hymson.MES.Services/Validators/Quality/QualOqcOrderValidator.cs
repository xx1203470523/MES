using FluentValidation;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// OQC检验单 验证
    /// </summary>
    internal class QualOqcOrderSaveValidator : AbstractValidator<QualOqcOrderSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualOqcOrderSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
