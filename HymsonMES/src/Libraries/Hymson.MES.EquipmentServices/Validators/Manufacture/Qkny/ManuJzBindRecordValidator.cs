using FluentValidation;
using Hymson.MES.Services.Dtos.ManuJzBindRecord;

namespace Hymson.MES.Services.Validators.ManuJzBindRecord
{
    /// <summary>
    /// 极组绑定记录 验证
    /// </summary>
    internal class ManuJzBindRecordSaveValidator: AbstractValidator<ManuJzBindRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManuJzBindRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
