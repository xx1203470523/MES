using FluentValidation;
using Hymson.MES.Services.Dtos.CcdFileUploadCompleteRecord;

namespace Hymson.MES.Services.Validators.CcdFileUploadCompleteRecord
{
    /// <summary>
    /// CCD文件上传完成 验证
    /// </summary>
    internal class CcdFileUploadCompleteRecordSaveValidator: AbstractValidator<CcdFileUploadCompleteRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public CcdFileUploadCompleteRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
