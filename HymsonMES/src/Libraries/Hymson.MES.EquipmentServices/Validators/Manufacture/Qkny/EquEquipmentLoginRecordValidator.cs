using FluentValidation;
using Hymson.MES.Services.Dtos.EquEquipmentLoginRecord;

namespace Hymson.MES.Services.Validators.EquEquipmentLoginRecord
{
    /// <summary>
    /// 操作员登录记录 验证
    /// </summary>
    internal class EquEquipmentLoginRecordSaveValidator: AbstractValidator<EquEquipmentLoginRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquEquipmentLoginRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
