using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// OQC检验参数组 验证
    /// </summary>
    internal class QualOqcParameterGroupSaveValidator : AbstractValidator<QualOqcParameterGroupDto>
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

 

    /// <summary>
    /// OQC检验项目 ; 更新校验器
    /// 描述：
    /// </summary>
    /// <returns></returns>
    internal class QualOqcParameterGroupUpdateValidator : AbstractValidator<QualOqcParameterGroupUpdateDto>
    {
        public QualOqcParameterGroupUpdateValidator()
        {
            RuleFor(m => m.Id).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
        }
    }

    /// <summary>
    /// OQC检验项目 ; 删除校验器
    /// 描述：
    /// </summary>
    /// <returns></returns>
    internal class QualOqcParameterGroupDeleteValidator : AbstractValidator<QualOqcParameterGroupDeleteDto>
    {
        public QualOqcParameterGroupDeleteValidator()
        {
            RuleFor(m => m.Ids).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
        }
    }

}
