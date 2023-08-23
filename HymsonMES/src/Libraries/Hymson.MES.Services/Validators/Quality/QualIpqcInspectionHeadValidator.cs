using FluentValidation;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// 首检检验单 验证
    /// </summary>
    internal class QualIpqcInspectionHeadSaveValidator: AbstractValidator<QualIpqcInspectionHeadSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualIpqcInspectionHeadSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
