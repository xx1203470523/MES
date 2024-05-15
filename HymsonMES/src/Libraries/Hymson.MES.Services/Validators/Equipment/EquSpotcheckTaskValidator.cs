using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 点检任务 验证
    /// </summary>
    internal class EquSpotcheckTaskSaveValidator: AbstractValidator<EquSpotcheckTaskSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquSpotcheckTaskSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
