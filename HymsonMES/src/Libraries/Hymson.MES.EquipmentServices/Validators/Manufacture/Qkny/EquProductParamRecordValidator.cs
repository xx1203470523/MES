using FluentValidation;
using Hymson.MES.Services.Dtos.EquProductParamRecord;

namespace Hymson.MES.Services.Validators.EquProductParamRecord
{
    /// <summary>
    /// 产品参数记录表 验证
    /// </summary>
    internal class EquProductParamRecordSaveValidator: AbstractValidator<EquProductParamRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquProductParamRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
