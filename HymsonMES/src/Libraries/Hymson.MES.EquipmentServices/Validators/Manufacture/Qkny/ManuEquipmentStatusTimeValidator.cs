using FluentValidation;
using Hymson.MES.Services.Dtos.ManuEquipmentStatusTime;

namespace Hymson.MES.Services.Validators.ManuEquipmentStatusTime
{
    /// <summary>
    /// 设备状态时间 验证
    /// </summary>
    internal class ManuEquipmentStatusTimeSaveValidator: AbstractValidator<ManuEquipmentStatusTimeSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManuEquipmentStatusTimeSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
