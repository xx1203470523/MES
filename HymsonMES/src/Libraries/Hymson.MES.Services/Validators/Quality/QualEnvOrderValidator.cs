/*
 *creator: Karl
 *
 *describe: 环境检验单    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:53
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.QualEnvOrder;

namespace Hymson.MES.Services.Validators.QualEnvOrder
{
    /// <summary>
    /// 环境检验单 更新 验证
    /// </summary>
    internal class QualEnvOrderCreateValidator: AbstractValidator<QualEnvOrderCreateDto>
    {
        public QualEnvOrderCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 环境检验单 修改 验证
    /// </summary>
    internal class QualEnvOrderModifyValidator : AbstractValidator<QualEnvOrderModifyDto>
    {
        public QualEnvOrderModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
