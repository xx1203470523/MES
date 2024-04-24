using FluentValidation;
using Hymson.MES.Services.Dtos.EquToolLifeRecord;

namespace Hymson.MES.Services.Validators.EquToolLifeRecord
{
    /// <summary>
    /// 设备夹具寿命 验证
    /// </summary>
    internal class EquToolLifeRecordSaveValidator: AbstractValidator<EquToolLifeRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquToolLifeRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
