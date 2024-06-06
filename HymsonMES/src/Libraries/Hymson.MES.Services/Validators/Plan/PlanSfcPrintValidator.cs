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

        }
    }
    internal class PlanSfcPrintCreatePrintValidator : AbstractValidator<PlanSfcPrintCreatePrintDto>
    {
        public PlanSfcPrintCreatePrintValidator()
        {

        }
    }

    internal class WhMaterialSfcPrintCreatePrintValidator : AbstractValidator<WhMaterialInventoryPrintCreatePrintDto>
    {
        public WhMaterialSfcPrintCreatePrintValidator()
        {

        }
    }

}
