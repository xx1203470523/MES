using FluentValidation;
using Hymson.MES.Services.Dtos.ManuFeedingNoProductionRecord;

namespace Hymson.MES.Services.Validators.ManuFeedingNoProductionRecord
{
    /// <summary>
    /// 设备投料非生产投料(洗罐子) 验证
    /// </summary>
    internal class ManuFeedingNoProductionRecordSaveValidator: AbstractValidator<ManuFeedingNoProductionRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManuFeedingNoProductionRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
