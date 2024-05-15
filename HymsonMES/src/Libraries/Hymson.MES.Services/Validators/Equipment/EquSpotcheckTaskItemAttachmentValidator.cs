using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 设备点检任务项目附件 验证
    /// </summary>
    internal class EquSpotcheckTaskItemAttachmentSaveValidator: AbstractValidator<EquSpotcheckTaskItemAttachmentSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquSpotcheckTaskItemAttachmentSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
