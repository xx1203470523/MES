using FluentValidation;
using Hymson.MES.Services.Dtos.WhShipment;

namespace Hymson.MES.Services.Validators.WhShipment
{
    /// <summary>
    /// 出货单 验证
    /// </summary>
    internal class WhShipmentSaveValidator: AbstractValidator<WhShipmentSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public WhShipmentSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
