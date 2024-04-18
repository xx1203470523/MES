using FluentValidation;
using Hymson.MES.Services.Dtos.ManuFillingDataRecord;

namespace Hymson.MES.Services.Validators.ManuFillingDataRecord
{
    /// <summary>
    /// 补液数据上传记录 验证
    /// </summary>
    internal class ManuFillingDataRecordSaveValidator: AbstractValidator<ManuFillingDataRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManuFillingDataRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
