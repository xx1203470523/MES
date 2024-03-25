using FluentValidation;
using Hymson.MES.Services.Dtos.ManuFeedingTransferRecord;

namespace Hymson.MES.Services.Validators.ManuFeedingTransferRecord
{
    /// <summary>
    /// 上料信息转移记录 验证
    /// </summary>
    internal class ManuFeedingTransferRecordSaveValidator: AbstractValidator<ManuFeedingTransferRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManuFeedingTransferRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
