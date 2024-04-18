using FluentValidation;
using Hymson.MES.Services.Dtos.ManuJzBind;

namespace Hymson.MES.EquipmentServices.Validators.Manufacture.Qkny
{
    /// <summary>
    /// 极组绑定 验证
    /// </summary>
    internal class ManuJzBindSaveValidator : AbstractValidator<ManuJzBindSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManuJzBindSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
