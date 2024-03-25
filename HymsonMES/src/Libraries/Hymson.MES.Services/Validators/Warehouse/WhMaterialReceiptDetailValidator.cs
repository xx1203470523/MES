using FluentValidation;
using Hymson.MES.Services.Dtos.WHMaterialReceiptDetail;

namespace Hymson.MES.Services.Validators.WhMaterialReceiptDetail
{
    /// <summary>
    /// 收料单详情 验证
    /// </summary>
    internal class WhMaterialReceiptDetailSaveValidator: AbstractValidator<WHMaterialReceiptDetailSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public WhMaterialReceiptDetailSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
