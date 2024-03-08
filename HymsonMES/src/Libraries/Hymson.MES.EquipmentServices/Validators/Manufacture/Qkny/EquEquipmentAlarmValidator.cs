using FluentValidation;
using Hymson.MES.Services.Dtos.EquEquipmentAlarm;

namespace Hymson.MES.Services.Validators.EquEquipmentAlarm
{
    /// <summary>
    /// 设备报警记录 验证
    /// </summary>
    internal class EquEquipmentAlarmSaveValidator: AbstractValidator<EquEquipmentAlarmSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquEquipmentAlarmSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
