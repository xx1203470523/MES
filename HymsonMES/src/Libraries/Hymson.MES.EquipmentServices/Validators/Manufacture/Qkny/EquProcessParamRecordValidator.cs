using FluentValidation;
using Hymson.MES.Services.Dtos.EquProcessParamRecord;

namespace Hymson.MES.Services.Validators.EquProcessParamRecord
{
    /// <summary>
    /// 过程参数记录表 验证
    /// </summary>
    internal class EquProcessParamRecordSaveValidator: AbstractValidator<EquProcessParamRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquProcessParamRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
