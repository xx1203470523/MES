using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// OQC检验参数组 验证
    /// </summary>
    internal class QualFqcParameterGroupSaveValidator : AbstractValidator<QualFqcParameterGroupDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualFqcParameterGroupSaveValidator()
        {
            //RuleFor(x => x.Code).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

 

    /// <summary>
    /// OQC检验项目 ; 更新校验器
    /// 描述：
    /// </summary>
    /// <returns></returns>
    internal class QualFqcParameterGroupUpdateValidator : AbstractValidator<QualFqcParameterGroupUpdateDto>
    {
        public QualFqcParameterGroupUpdateValidator()
        {
            RuleFor(m => m.Id).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
        }
    }

    /// <summary>
    /// OQC检验项目 ; 删除校验器
    /// 描述：
    /// </summary>
    /// <returns></returns>
    internal class QualFqcParameterGroupDeleteValidator : AbstractValidator<QualFqcParameterGroupDeleteDto>
    {
        public QualFqcParameterGroupDeleteValidator()
        {
            RuleFor(m => m.Ids).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
        }
    }

}
