using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 设备点检任务结果处理 验证
    /// </summary>
    internal class EquSpotcheckTaskProcessedSaveValidator: AbstractValidator<EquSpotcheckTaskProcessedSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquSpotcheckTaskProcessedSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
