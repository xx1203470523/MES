using FluentValidation;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// OQC检验参数组 验证
    /// </summary>
    internal class QualOqcParameterGroupSaveValidator : AbstractValidator<QualOqcParameterGroupSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualOqcParameterGroupSaveValidator()
        {
            //RuleFor(x => x.Code).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
