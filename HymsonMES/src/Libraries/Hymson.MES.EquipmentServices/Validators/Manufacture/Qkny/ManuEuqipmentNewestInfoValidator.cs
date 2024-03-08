using FluentValidation;
using Hymson.MES.Services.Dtos.ManuEuqipmentNewestInfo;

namespace Hymson.MES.Services.Validators.ManuEuqipmentNewestInfo
{
    /// <summary>
    /// 设备最新信息 验证
    /// </summary>
    internal class ManuEuqipmentNewestInfoSaveValidator: AbstractValidator<ManuEuqipmentNewestInfoSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManuEuqipmentNewestInfoSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
