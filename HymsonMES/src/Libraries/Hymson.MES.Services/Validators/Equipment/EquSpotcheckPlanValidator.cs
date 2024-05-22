/*
 *creator: Karl
 *
 *describe: 设备点检计划    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-05-16 02:14:30
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.EquSpotcheckPlan;

namespace Hymson.MES.Services.Validators.EquSpotcheckPlan
{
    /// <summary>
    /// 设备点检计划 更新 验证
    /// </summary>
    internal class EquSpotcheckPlanCreateValidator: AbstractValidator<EquSpotcheckPlanCreateDto>
    {
        public EquSpotcheckPlanCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 设备点检计划 修改 验证
    /// </summary>
    internal class EquSpotcheckPlanModifyValidator : AbstractValidator<EquSpotcheckPlanModifyDto>
    {
        public EquSpotcheckPlanModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
