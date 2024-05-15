using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 设备点检任务操作 验证
    /// </summary>
    internal class EquSpotcheckTaskOperationSaveValidator: AbstractValidator<EquSpotcheckTaskOperationSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquSpotcheckTaskOperationSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
