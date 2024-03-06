using FluentValidation;
using Hymson.MES.Services.Dtos.WHMaterialReceipt;

namespace Hymson.MES.Services.Validators.WHMaterialReceipt
{
    /// <summary>
    /// 物料收货表 验证
    /// </summary>
    internal class WhMaterialReceiptSaveValidator: AbstractValidator<WhMaterialReceiptSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public WhMaterialReceiptSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
