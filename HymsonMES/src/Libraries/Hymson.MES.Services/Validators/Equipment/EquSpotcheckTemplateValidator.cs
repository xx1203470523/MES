/*
 *creator: Karl
 *
 *describe: 设备点检模板    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.EquSpotcheckTemplate;

namespace Hymson.MES.Services.Validators.EquSpotcheckTemplate
{
    /// <summary>
    /// 设备点检模板 更新 验证
    /// </summary>
    internal class EquSpotcheckTemplateCreateValidator: AbstractValidator<EquSpotcheckTemplateCreateDto>
    {
        public EquSpotcheckTemplateCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 设备点检模板 修改 验证
    /// </summary>
    internal class EquSpotcheckTemplateModifyValidator : AbstractValidator<EquSpotcheckTemplateModifyDto>
    {
        public EquSpotcheckTemplateModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
