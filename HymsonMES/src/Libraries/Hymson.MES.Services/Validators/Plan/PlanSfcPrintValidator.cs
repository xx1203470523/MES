using FluentValidation;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Validators.Plan
{
    /// <summary>
    /// 条码打印 更新 验证
    /// </summary>
    internal class PlanSfcPrintCreateValidator : AbstractValidator<PlanSfcPrintCreateDto>
    {
        public PlanSfcPrintCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

}
