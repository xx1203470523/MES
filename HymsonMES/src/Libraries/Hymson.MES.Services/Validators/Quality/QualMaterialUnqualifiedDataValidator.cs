using FluentValidation;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// 车间物料不良记录 验证
    /// </summary>
    internal class QualMaterialUnqualifiedDataSaveValidator: AbstractValidator<QualMaterialUnqualifiedDataSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualMaterialUnqualifiedDataSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
