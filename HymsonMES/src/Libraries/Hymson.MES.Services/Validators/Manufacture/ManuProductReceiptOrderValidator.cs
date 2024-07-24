using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 工单完工入库 验证
    /// </summary>
    internal class ManuProductReceiptOrderSaveValidator: AbstractValidator<ManuProductReceiptOrderSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManuProductReceiptOrderSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
