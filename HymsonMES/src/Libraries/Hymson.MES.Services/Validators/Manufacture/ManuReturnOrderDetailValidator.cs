using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 生产退料单明细 验证
    /// </summary>
    internal class ManuReturnOrderDetailSaveValidator: AbstractValidator<ManuReturnOrderDetailSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManuReturnOrderDetailSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
