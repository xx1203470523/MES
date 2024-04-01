/*
 *creator: Karl
 *
 *describe: 环境检验单检验明细    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:43
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.QualEnvOrderDetail;

namespace Hymson.MES.Services.Validators.QualEnvOrderDetail
{
    /// <summary>
    /// 环境检验单检验明细 更新 验证
    /// </summary>
    internal class QualEnvOrderDetailCreateValidator : AbstractValidator<QualEnvOrderDetailCreateDto>
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


    /// <summary>
    /// 环境检验单检验明细 修改 验证 (批量)
    /// </summary>
    internal class QualEnvOrderDetailModifysValidator : AbstractValidator<List<QualEnvOrderDetailModifyDto>>
    {
        public QualEnvOrderDetailModifysValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13604));

            //When(x => x != null && x.Any(), () =>
            //{
            //    RuleForEach(x => x).ChildRules(c =>
            //    {
            //c.RuleFor(x => x.IsQualified).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13600));
            //        c.RuleFor(x => x.InspectionValue).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13602));
            //        c.RuleFor(x => x.RealTime).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13603));
            //    });
            //});

        }
    }
}
