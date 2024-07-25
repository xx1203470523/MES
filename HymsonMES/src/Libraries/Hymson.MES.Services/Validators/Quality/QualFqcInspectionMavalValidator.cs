/*
 *creator: Karl
 *
 *describe: 马威FQC检验    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-07-24 03:09:40
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.QualFqcInspectionMaval;

namespace Hymson.MES.Services.Validators.QualFqcInspectionMaval
{
    /// <summary>
    /// 马威FQC检验 更新 验证
    /// </summary>
    internal class QualFqcInspectionMavalCreateValidator: AbstractValidator<QualFqcInspectionMavalCreateDto>
    {
        public QualFqcInspectionMavalCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 马威FQC检验 修改 验证
    /// </summary>
    internal class QualFqcInspectionMavalModifyValidator : AbstractValidator<QualFqcInspectionMavalModifyDto>
    {
        public QualFqcInspectionMavalModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
