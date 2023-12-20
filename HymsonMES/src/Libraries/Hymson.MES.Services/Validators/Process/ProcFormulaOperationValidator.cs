using FluentValidation;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 配方操作 验证
    /// </summary>
    internal class ProcFormulaOperationSaveValidator: AbstractValidator<ProcFormulaOperationSaveDto>
    {
        public ProcFormulaOperationSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
