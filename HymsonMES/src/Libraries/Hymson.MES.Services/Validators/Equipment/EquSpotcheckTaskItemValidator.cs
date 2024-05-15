using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 设备点检任务项目 验证
    /// </summary>
    internal class EquSpotcheckTaskItemSaveValidator: AbstractValidator<EquSpotcheckTaskItemSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquSpotcheckTaskItemSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
