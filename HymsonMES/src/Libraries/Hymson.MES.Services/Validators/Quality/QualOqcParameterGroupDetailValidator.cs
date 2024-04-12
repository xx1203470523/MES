using FluentValidation;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// OQC检验参数组明细 验证
    /// </summary>
    internal class QualOqcParameterGroupDetailSaveValidator: AbstractValidator<QualOqcParameterGroupDetailSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualOqcParameterGroupDetailSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
