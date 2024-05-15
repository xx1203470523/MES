using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 设备点检任务附件 验证
    /// </summary>
    internal class EquSpotcheckTaskAttachmentSaveValidator: AbstractValidator<EquSpotcheckTaskAttachmentSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquSpotcheckTaskAttachmentSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
