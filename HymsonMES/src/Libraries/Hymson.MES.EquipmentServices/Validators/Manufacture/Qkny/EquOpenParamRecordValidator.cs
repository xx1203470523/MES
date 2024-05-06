using FluentValidation;
using Hymson.MES.Services.Dtos.EquOpenParamRecord;

namespace Hymson.MES.EquipmentServices.Validators.Manufacture.Qkny
{
    /// <summary>
    /// 开机参数记录表 验证
    /// </summary>
    internal class EquOpenParamRecordSaveValidator : AbstractValidator<EquOpenParamRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquOpenParamRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
