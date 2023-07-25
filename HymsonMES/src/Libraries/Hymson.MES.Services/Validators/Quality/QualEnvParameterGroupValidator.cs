using FluentValidation;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// 环境检验参数表 验证
    /// </summary>
    internal class QualEnvParameterGroupSaveValidator: AbstractValidator<QualEnvParameterGroupSaveDto>
    {
        public QualEnvParameterGroupSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
