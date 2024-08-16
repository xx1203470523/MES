using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 废成品入库记录 验证
    /// </summary>
    internal class ManuWasteProductsReceiptRecordSaveValidator: AbstractValidator<ManuWasteProductsReceiptRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManuWasteProductsReceiptRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
