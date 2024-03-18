using FluentValidation;
using Hymson.MES.Services.Dtos.ManuFeedingCompletedZjyjRecord;

namespace Hymson.MES.Services.Validators.ManuFeedingCompletedZjyjRecord
{
    /// <summary>
    /// manu_feeding_completed_zjyj_record 验证
    /// </summary>
    internal class ManuFeedingCompletedZjyjRecordSaveValidator: AbstractValidator<ManuFeedingCompletedZjyjRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManuFeedingCompletedZjyjRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
