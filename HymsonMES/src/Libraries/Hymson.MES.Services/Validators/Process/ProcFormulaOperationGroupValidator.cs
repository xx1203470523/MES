using FluentValidation;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 配方操作组 验证
    /// </summary>
    internal class ProcFormulaOperationGroupSaveValidator: AbstractValidator<ProcFormulaOperationGroupSaveDto>
    {
        public ProcFormulaOperationGroupSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
