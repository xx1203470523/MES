using FluentValidation;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// IPQC检验项目 验证
    /// </summary>
    internal class QualIpqcInspectionSaveValidator: AbstractValidator<QualIpqcInspectionSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualIpqcInspectionSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
