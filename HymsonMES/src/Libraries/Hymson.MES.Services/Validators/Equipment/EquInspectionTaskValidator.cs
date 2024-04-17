using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 点检任务 验证
    /// </summary>
    internal class EquInspectionTaskSaveValidator: AbstractValidator<EquInspectionTaskSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquInspectionTaskSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
