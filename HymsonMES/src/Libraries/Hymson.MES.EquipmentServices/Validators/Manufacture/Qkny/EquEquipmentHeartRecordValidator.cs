using FluentValidation;
using Hymson.MES.Services.Dtos.EquEquipmentHeartRecord;

namespace Hymson.MES.Services.Validators.EquEquipmentHeartRecord
{
    /// <summary>
    /// 设备心跳登录记录 验证
    /// </summary>
    internal class EquEquipmentHeartRecordSaveValidator: AbstractValidator<EquEquipmentHeartRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquEquipmentHeartRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
