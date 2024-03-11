using FluentValidation;
using Hymson.MES.Services.Dtos.AgvTaskRecord;

namespace Hymson.MES.Services.Validators.AgvTaskRecord
{
    /// <summary>
    /// AGV任务记录表 验证
    /// </summary>
    internal class AgvTaskRecordSaveValidator: AbstractValidator<AgvTaskRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public AgvTaskRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
