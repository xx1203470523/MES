/*
 *creator: Karl
 *
 *describe: 环境检验单检验明细    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:43
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.QualEnvOrderDetail;

namespace Hymson.MES.Services.Validators.QualEnvOrderDetail
{
    /// <summary>
    /// 环境检验单检验明细 更新 验证
    /// </summary>
    internal class QualEnvOrderDetailCreateValidator: AbstractValidator<QualEnvOrderDetailCreateDto>
    {
        public QualEnvOrderDetailCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 环境检验单检验明细 修改 验证
    /// </summary>
    internal class QualEnvOrderDetailModifyValidator : AbstractValidator<QualEnvOrderDetailModifyDto>
    {
        public QualEnvOrderDetailModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
