using FluentValidation;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// 全检参数表 验证
    /// </summary>
    internal class QualInspectionParameterGroupSaveValidator: AbstractValidator<QualInspectionParameterGroupSaveDto>
    {
        public QualInspectionParameterGroupSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
